using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.PostgreSQL;
using DotNetDBTools.Definition.PostgreSQL.UserDefinedTypes;
using DotNetDBTools.DefinitionParsing.Common;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.DefinitionParsing.PostgreSQL;

internal class PostgreSQLDbModelFromCSharpDefinitionProvider : DbModelFromCSharpDefinitionProvider<
    PostgreSQLDatabase,
    PostgreSQLTable,
    PostgreSQLView,
    Models.Core.Column>
{
    public PostgreSQLDbModelFromCSharpDefinitionProvider() : base(
        new PostgreSQLDataTypeMapper(),
        new SpecificDbmsDbObjectCodeMapper(),
        new PostgreSQLDefaultValueMapper())
    {
    }

    protected override void BuildAdditionalDbObjects(Database database, Assembly dbAssembly)
    {
        PostgreSQLDatabase db = (PostgreSQLDatabase)database;
        db.CompositeTypes = BuildCompositeTypeModels(dbAssembly);
        db.DomainTypes = BuildDomainModels(dbAssembly);
        db.EnumTypes = BuildEnumTypeModels(dbAssembly);
        db.RangeTypes = BuildRangeTypeModels(dbAssembly);
        db.Functions = BuildFunctionModels(dbAssembly);
        db.Procedures = new();
    }

    protected override void BuildAdditionalTableModelProperties(PostgreSQLTable tableModel, IBaseTable table)
    {
        if (table is ITypedTable typedTable)
            tableModel.OfType = typedTable.OfType;
    }

    protected override void BuildAdditionalIndexModelProperties(
        Models.Core.Index indexModel, BaseIndex index, string tableName)
    {
        indexModel.IncludeColumns = ((Definition.PostgreSQL.Index)index).IncludeColumns?.ToList() ?? new List<string>();
    }

    protected override string GetOnUpdateActionName(BaseForeignKey fk) =>
        MapFKActionNameFromDefinitionToModel(((Definition.PostgreSQL.ForeignKey)fk).OnUpdate.ToString());
    protected override string GetOnDeleteActionName(BaseForeignKey fk) =>
        MapFKActionNameFromDefinitionToModel(((Definition.PostgreSQL.ForeignKey)fk).OnDelete.ToString());

    private List<PostgreSQLCompositeType> BuildCompositeTypeModels(Assembly dbAssembly)
    {
        IEnumerable<ICompositeType> typesList = GetInstancesOfAllTypesImplementingInterface<ICompositeType>(dbAssembly);
        List<PostgreSQLCompositeType> typeModelsList = new();
        foreach (ICompositeType type in typesList)
        {
            PostgreSQLCompositeType typeModel = new()
            {
                ID = type.ID,
                Name = type.GetType().Name,
                Attributes = type.Attributes.Select(x => new PostgreSQLCompositeTypeAttribute
                {
                    Name = x.Key,
                    DataType = DataTypeMapper.MapToDataTypeModel(x.Value),
                }).ToList(),
            };
            typeModelsList.Add(typeModel);
        }
        return typeModelsList;
    }

    private List<PostgreSQLDomainType> BuildDomainModels(Assembly dbAssembly)
    {
        IEnumerable<IDomain> typesList = GetInstancesOfAllTypesImplementingInterface<IDomain>(dbAssembly);
        List<PostgreSQLDomainType> typeModelsList = new();
        foreach (IDomain type in typesList)
        {
            string typeName = type.GetType().Name;
            PostgreSQLDomainType typeModel = new()
            {
                ID = type.ID,
                Name = typeName,
                UnderlyingType = DataTypeMapper.MapToDataTypeModel(type.UnderlyingType),
                NotNull = type.NotNull,
                Default = DefaultValueMapper.MapToDefaultValueModel(type.Default),
                CheckConstraints = BuildCheckConstraintModels(type),
            };
            typeModelsList.Add(typeModel);
        }
        return typeModelsList;

        List<Models.Core.CheckConstraint> BuildCheckConstraintModels(IDomain type)
        {
            return type.GetType().GetPropertyOrFieldMembers()
            .Where(x => typeof(BaseCheckConstraint).IsAssignableFrom(x.GetPropertyOrFieldType()))
            .OrderBy(x => x.Name, StringComparer.Ordinal)
            .Select(x =>
            {
                BaseCheckConstraint ck = (BaseCheckConstraint)x.GetPropertyOrFieldValue(type);
                Models.Core.CheckConstraint ckModel = new()
                {
                    ID = ck.ID,
                    Name = x.Name,
                    CodePiece = DbObjectCodeMapper.MapToCodePiece(ck),
                };
                return ckModel;
            })
            .ToList();
        }
    }

    private List<PostgreSQLEnumType> BuildEnumTypeModels(Assembly dbAssembly)
    {
        IEnumerable<IEnumType> typesList = GetInstancesOfAllTypesImplementingInterface<IEnumType>(dbAssembly);
        List<PostgreSQLEnumType> typeModelsList = new();
        foreach (IEnumType type in typesList)
        {
            PostgreSQLEnumType typeModel = new()
            {
                ID = type.ID,
                Name = type.GetType().Name,
                AllowedValues = type.AllowedValues.ToList(),
            };
            typeModelsList.Add(typeModel);
        }
        return typeModelsList;
    }

    private List<PostgreSQLRangeType> BuildRangeTypeModels(Assembly dbAssembly)
    {
        IEnumerable<IRangeType> typesList = GetInstancesOfAllTypesImplementingInterface<IRangeType>(dbAssembly);
        List<PostgreSQLRangeType> typeModelsList = new();
        foreach (IRangeType type in typesList)
        {
            string typeName = type.GetType().Name;
            DataType subtype = DataTypeMapper.MapToDataTypeModel(type.Subtype);
            subtype.Name = subtype.Name.Split('[')[0].Split('(')[0];
            PostgreSQLRangeType typeModel = new()
            {
                ID = type.ID,
                Name = typeName,
                Subtype = subtype,
                SubtypeOperatorClass = type.SubtypeOperatorClass ?? GetDefaultSubtypeOperatorClass(subtype),
                Collation = type.Collation ?? GetDefaultCollation(subtype),
                CanonicalFunction = type.CanonicalFunction ?? null,
                SubtypeDiff = type.SubtypeDiff ?? null,
                MultirangeTypeName = type.MultirangeTypeName ?? $"{typeName}_multirange",
            };
            typeModelsList.Add(typeModel);
        }
        return typeModelsList;

        string GetDefaultSubtypeOperatorClass(DataType subtype)
        {
            return subtype.Name switch
            {
                PostgreSQLDataTypeNames.SMALLINT => "int2_ops",
                PostgreSQLDataTypeNames.INT => "int4_ops",
                PostgreSQLDataTypeNames.BIGINT => "int8_ops",
                PostgreSQLDataTypeNames.FLOAT4 => "float4_ops",
                PostgreSQLDataTypeNames.FLOAT8 => "float8_ops",
                PostgreSQLDataTypeNames.DECIMAL => "numeric_ops",
                PostgreSQLDataTypeNames.BOOL => "bool_ops",
                PostgreSQLDataTypeNames.MONEY => "money_ops",
                PostgreSQLDataTypeNames.CHAR => "char_ops",
                PostgreSQLDataTypeNames.VARCHAR => "text_ops",
                PostgreSQLDataTypeNames.TEXT => "text_ops",
                PostgreSQLDataTypeNames.BYTEA => "bytea_ops",
                PostgreSQLDataTypeNames.DATE => "date_ops",
                PostgreSQLDataTypeNames.TIME => "time_ops",
                PostgreSQLDataTypeNames.TIMETZ => "timetz_ops",
                PostgreSQLDataTypeNames.TIMESTAMP => "timestamp_ops",
                PostgreSQLDataTypeNames.TIMESTAMPTZ => "timestamptz_ops",
                PostgreSQLDataTypeNames.UUID => "uuid_ops",
                PostgreSQLDataTypeNames.BIT => "bit_ops",
                PostgreSQLDataTypeNames.VARBIT => "varbit_ops",
                _ => null,
            };
        }

        string GetDefaultCollation(DataType subtype)
        {
            return subtype.Name switch
            {
                PostgreSQLDataTypeNames.CHAR => "default",
                PostgreSQLDataTypeNames.VARCHAR => "default",
                PostgreSQLDataTypeNames.TEXT => "default",
                _ => null,
            };
        }
    }

    private static List<PostgreSQLFunction> BuildFunctionModels(Assembly dbAssembly)
    {
        IEnumerable<IFunction> functions = GetInstancesOfAllTypesImplementingInterface<IFunction>(dbAssembly);
        List<PostgreSQLFunction> functionModels = new();
        foreach (IFunction function in functions)
        {
            PostgreSQLFunction functionModel = new()
            {
                ID = function.ID,
                Name = function.GetType().Name,
                CodePiece = new CodePiece { Code = function.Code.NormalizeLineEndings() },
            };
            functionModels.Add(functionModel);
        }
        return functionModels;
    }
}
