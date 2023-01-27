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
    PostgreSQLColumn>
{
    public PostgreSQLDbModelFromSqlDefinitionProvider() : base(
        new PostgreSQLCodeParser())
    {
    }

    protected override void BuildAdditionalDbObjects(Database database, List<ObjectInfo> dbObjects)
    {
        PostgreSQLDatabase db = (PostgreSQLDatabase)database;
        db.Sequences = BuildSequenceModels(dbObjects.OfType<SequenceInfo>());
        db.CompositeTypes = BuildCompositeTypeModels(dbObjects.OfType<TypeInfo>().Where(x => x.TypeType == TypeType.Composite));
        db.DomainTypes = BuildDomainModels(dbObjects.OfType<TypeInfo>().Where(x => x.TypeType == TypeType.Domain));
        db.EnumTypes = BuildEnumTypeModels(dbObjects.OfType<TypeInfo>().Where(x => x.TypeType == TypeType.Enum));
        db.RangeTypes = BuildRangeTypeModels(dbObjects.OfType<TypeInfo>().Where(x => x.TypeType == TypeType.Range));
        db.Functions = BuildFunctionModels(dbObjects.OfType<FunctionInfo>());
        db.Procedures = BuildProcedureModels(dbObjects.OfType<ProcedureInfo>());
    }

    protected override void BuildAdditionalColumnModelProperties(PostgreSQLColumn columnModel, ColumnInfo column, string tableName)
    {
        if (columnModel.Identity)
        {
            columnModel.IdentityGenerationKind = column.IdentityGenerationKind is null ? "ALWAYS" : column.IdentityGenerationKind;
            columnModel.IdentitySequenceOptions = MapToSequenceOptionsModel(column.IdentitySequenceOptions);
        }
    }

    protected override void BuildAdditionalIndexModelProperties(Index indexModel, IndexInfo index)
    {
        if (index.Expression is not null)
            ((PostgreSQLIndex)indexModel).Expression = new CodePiece() { Code = index.Expression };
    }

    private List<PostgreSQLSequence> BuildSequenceModels(IEnumerable<SequenceInfo> sequences)
    {
        List<PostgreSQLSequence> sequenceModels = new();
        foreach (SequenceInfo sequence in sequences)
        {
            PostgreSQLSequence sequenceModel = new()
            {
                ID = sequence.ID.Value,
                Name = sequence.Name,
                DataType = new DataType { Name = sequence.DataType ?? PostgreSQLDataTypeNames.INT },
                Options = MapToSequenceOptionsModel(sequence.Options),
                OwnedBy = (sequence.OwnedByTableName, sequence.OwnedByColumnName),
            };
            sequenceModels.Add(sequenceModel);
        }
        return sequenceModels;
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
                Default = type.Default is null ? null : new CodePiece { Code = type.Default },
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
                CreateStatement = new CodePiece { Code = function.CreateStatement.NormalizeLineEndings() },
            };
            functionModels.Add(functionModel);
        }
        return functionModels;
    }

    private List<PostgreSQLProcedure> BuildProcedureModels(IEnumerable<ProcedureInfo> procedures)
    {
        List<PostgreSQLProcedure> procedureModels = new();
        foreach (ProcedureInfo procedure in procedures)
        {
            PostgreSQLProcedure procedureModel = new()
            {
                ID = procedure.ID.Value,
                Name = procedure.Name,
                CreateStatement = new CodePiece { Code = procedure.CreateStatement.NormalizeLineEndings() },
            };
            procedureModels.Add(procedureModel);
        }
        return procedureModels;
    }

    private static PostgreSQLSequenceOptions MapToSequenceOptionsModel(SequenceOptions options)
    {
        return new PostgreSQLSequenceOptions()
        {
            StartWith = options.StartWith ?? 1,
            IncrementBy = options.IncrementBy ?? 1,
            MinValue = options.MinValue ?? 1,
            MaxValue = options.MaxValue ?? int.MaxValue,
            Cache = options.Cache ?? 1,
            Cycle = options.Cycle ?? false,
        };
    }
}
