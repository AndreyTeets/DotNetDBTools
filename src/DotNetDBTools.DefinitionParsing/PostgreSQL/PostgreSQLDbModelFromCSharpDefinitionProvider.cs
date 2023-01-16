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
    PostgreSQLIndex,
    PostgreSQLTrigger,
    PostgreSQLColumn>
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
        db.Sequences = BuildSequences(dbAssembly);
        db.CompositeTypes = BuildCompositeTypeModels(dbAssembly);
        db.DomainTypes = BuildDomainModels(dbAssembly);
        db.EnumTypes = BuildEnumTypeModels(dbAssembly);
        db.RangeTypes = BuildRangeTypeModels(dbAssembly);
        db.Functions = BuildFunctionModels(dbAssembly);
    }

    protected override void BuildAdditionalTableModelProperties(PostgreSQLTable tableModel, IBaseTable table)
    {
        if (table is ITypedTable typedTable)
            tableModel.OfType = typedTable.OfType;
    }

    protected override void BuildAdditionalColumnModelProperties(PostgreSQLColumn columnModel, BaseColumn column, string tableName)
    {
        if (columnModel.Identity)
        {
            Definition.PostgreSQL.Column c = (Definition.PostgreSQL.Column)column;
            columnModel.IdentityGenerationKind = c.IdentityGenerationKind == IdentityGenerationKind.Always ? "ALWAYS" : "BY DEFAULT";
            columnModel.IdentitySequenceOptions = MapToSequenceOptionsModel(c.IdentitySequenceOptions);
        }
    }

    protected override void BuildAdditionalIndexModelProperties(
        Models.Core.Index indexModel, BaseIndex index)
    {
        indexModel.IncludeColumns = ((Definition.PostgreSQL.Index)index).IncludeColumns?.ToList() ?? new List<string>();
    }

    protected override string GetOnUpdateActionName(BaseForeignKey fk) =>
        MapFKActionNameFromDefinitionToModel(((Definition.PostgreSQL.ForeignKey)fk).OnUpdate.ToString());
    protected override string GetOnDeleteActionName(BaseForeignKey fk) =>
        MapFKActionNameFromDefinitionToModel(((Definition.PostgreSQL.ForeignKey)fk).OnDelete.ToString());

    private List<PostgreSQLSequence> BuildSequences(Assembly dbAssembly)
    {
        IEnumerable<ISequence> sequencesList = GetInstancesOfAllTypesImplementingInterface<ISequence>(dbAssembly);
        List<PostgreSQLSequence> sequenceModelsList = new();
        foreach (ISequence sequence in sequencesList)
        {
            PostgreSQLSequence sequenceModel = new()
            {
                ID = sequence.DNDBT_OBJECT_ID,
                Name = sequence.GetType().Name,
                DataType = DataTypeMapper.MapToDataTypeModel(sequence.DataType),
                Options = MapToSequenceOptionsModel(sequence.Options),
                OwnedBy = sequence.OwnedBy,
            };
            sequenceModelsList.Add(sequenceModel);
        }
        return sequenceModelsList;
    }

    private List<PostgreSQLCompositeType> BuildCompositeTypeModels(Assembly dbAssembly)
    {
        IEnumerable<ICompositeType> typesList = GetInstancesOfAllTypesImplementingInterface<ICompositeType>(dbAssembly);
        List<PostgreSQLCompositeType> typeModelsList = new();
        foreach (ICompositeType type in typesList)
        {
            PostgreSQLCompositeType typeModel = new()
            {
                ID = type.DNDBT_OBJECT_ID,
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
                ID = type.DNDBT_OBJECT_ID,
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
                    ID = ck.DNDBT_OBJECT_ID,
                    Name = x.Name,
                    Expression = DbObjectCodeMapper.MapToCodePiece(ck),
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
                ID = type.DNDBT_OBJECT_ID,
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
                ID = type.DNDBT_OBJECT_ID,
                Name = typeName,
                Subtype = subtype,
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

    private static List<PostgreSQLFunction> BuildFunctionModels(Assembly dbAssembly)
    {
        IEnumerable<IFunction> functions = GetInstancesOfAllTypesImplementingInterface<IFunction>(dbAssembly);
        List<PostgreSQLFunction> functionModels = new();
        foreach (IFunction function in functions)
        {
            PostgreSQLFunction functionModel = new()
            {
                ID = function.DNDBT_OBJECT_ID,
                Name = function.GetType().Name,
                CreateStatement = new CodePiece { Code = function.CreateStatement.NormalizeLineEndings() },
            };
            functionModels.Add(functionModel);
        }
        return functionModels;
    }

    private static PostgreSQLSequenceOptions MapToSequenceOptionsModel(SequenceOptions options)
    {
        return new PostgreSQLSequenceOptions()
        {
            StartWith = options.StartWith,
            IncrementBy = options.IncrementBy,
            MinValue = options.MinValue,
            MaxValue = options.MaxValue,
            Cache = options.Cache,
            Cycle = options.Cycle,
        };
    }
}
