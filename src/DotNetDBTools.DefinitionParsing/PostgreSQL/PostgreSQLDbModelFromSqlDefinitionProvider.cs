using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.CodeParsing;
using DotNetDBTools.CodeParsing.Models;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.DefinitionParsing.PostgreSQL;

internal class PostgreSQLDbModelFromSqlDefinitionProvider : DbModelFromSqlDefinitionProvider<
    PostgreSQLDatabase,
    PostgreSQLTable,
    PostgreSQLView,
    PostgreSQLIndex,
    PostgreSQLTrigger,
    Column>
{
    public PostgreSQLDbModelFromSqlDefinitionProvider() : base(
        new PostgreSQLCodeParser())
    {
    }

    protected override void BuildAdditionalDbObjects(Database database, List<ObjectInfo> dbObjects)
    {
        PostgreSQLDatabase db = (PostgreSQLDatabase)database;
        db.CompositeTypes = BuildCompositeTypeModels(dbObjects.OfType<TypeInfo>().Where(x => x.TypeType == TypeType.Composite));
        db.DomainTypes = BuildDomainModels(dbObjects.OfType<TypeInfo>().Where(x => x.TypeType == TypeType.Domain));
        db.EnumTypes = BuildEnumTypeModels(dbObjects.OfType<TypeInfo>().Where(x => x.TypeType == TypeType.Enum));
        db.RangeTypes = BuildRangeTypeModels(dbObjects.OfType<TypeInfo>().Where(x => x.TypeType == TypeType.Range));
        db.Functions = BuildFunctionModels(dbObjects.OfType<FunctionInfo>());
    }

    private List<PostgreSQLCompositeType> BuildCompositeTypeModels(IEnumerable<TypeInfo> types)
    {
        List<PostgreSQLCompositeType> typeModelsList = new();
        foreach (TypeInfo type in types)
        {
            PostgreSQLCompositeType typeModel = new()
            {
                ID = type.ID.Value,
                Name = type.Name,
                Attributes = type.Attributes.Select(x => new PostgreSQLCompositeTypeAttribute
                {
                    Name = x.Key,
                    DataType = new DataType { Name = x.Value },
                }).ToList(),
            };
            typeModelsList.Add(typeModel);
        }
        return typeModelsList;
    }

    private List<PostgreSQLDomainType> BuildDomainModels(IEnumerable<TypeInfo> types)
    {
        List<PostgreSQLDomainType> typeModelsList = new();
        foreach (TypeInfo type in types)
        {
            PostgreSQLDomainType typeModel = new()
            {
                ID = type.ID.Value,
                Name = type.Name,
                UnderlyingType = new DataType { Name = type.UnderlyingType },
                NotNull = type.NotNull,
                Default = new CodePiece { Code = type.Default },
                CheckConstraints = BuildCheckConstraintModels(type),
            };
            typeModelsList.Add(typeModel);
        }
        return typeModelsList;

        static List<CheckConstraint> BuildCheckConstraintModels(TypeInfo type)
        {
            return type.CheckConstraints.Select(ck =>
            {
                CheckConstraint ckModel = new()
                {
                    ID = ck.ID.Value,
                    Name = ck.Name,
                    Expression = new CodePiece { Code = ck.Expression },
                };
                return ckModel;
            })
            .ToList();
        }
    }

    private List<PostgreSQLEnumType> BuildEnumTypeModels(IEnumerable<TypeInfo> types)
    {
        List<PostgreSQLEnumType> typeModelsList = new();
        foreach (TypeInfo type in types)
        {
            PostgreSQLEnumType typeModel = new()
            {
                ID = type.ID.Value,
                Name = type.Name,
                AllowedValues = type.AllowedValues.ToList(),
            };
            typeModelsList.Add(typeModel);
        }
        return typeModelsList;
    }

    private List<PostgreSQLRangeType> BuildRangeTypeModels(IEnumerable<TypeInfo> types)
    {
        List<PostgreSQLRangeType> typeModelsList = new();
        foreach (TypeInfo type in types)
        {
            PostgreSQLRangeType typeModel = new()
            {
                ID = type.ID.Value,
                Name = type.Name,
                Subtype = new DataType { Name = type.Subtype },
                SubtypeOperatorClass = type.SubtypeOperatorClass,
                Collation = type.Collation,
                CanonicalFunction = type.CanonicalFunction,
                SubtypeDiff = type.SubtypeDiff,
                MultirangeTypeName = type.MultirangeTypeName,
            };
            typeModelsList.Add(typeModel);
        }
        return typeModelsList;
    }

    private List<PostgreSQLFunction> BuildFunctionModels(IEnumerable<FunctionInfo> functions)
    {
        List<PostgreSQLFunction> functionModels = new();
        foreach (FunctionInfo function in functions)
        {
            PostgreSQLFunction functionModel = new()
            {
                ID = function.ID.Value,
                Name = function.Name,
                CodePiece = new CodePiece { Code = function.Code.NormalizeLineEndings() },
            };
            functionModels.Add(functionModel);
        }
        return functionModels;
    }
}
