using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;
using static DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo.GetDNDBTDbObjectRecordsQuery;
using static DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo.PostgreSQLGetCompositeTypesFromDBMSSysInfoQuery;
using static DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo.PostgreSQLGetDomainTypesFromDBMSSysInfoQuery;
using static DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo.PostgreSQLGetEnumTypesFromDBMSSysInfoQuery;
using static DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo.PostgreSQLGetFunctionsFromDBMSSysInfoQuery;
using static DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo.PostgreSQLGetRangeTypesFromDBMSSysInfoQuery;
using static DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo.PostgreSQLGetSequencesFromDBMSSysInfoQuery;

namespace DotNetDBTools.Deploy.PostgreSQL;

internal class PostgreSQLDbModelFromDBMSProvider : DbModelFromDBMSProvider<
    PostgreSQLDatabase,
    PostgreSQLTable,
    PostgreSQLView,
    PostgreSQLGetColumnsFromDBMSSysInfoQuery,
    PostgreSQLGetPrimaryKeysFromDBMSSysInfoQuery,
    PostgreSQLGetUniqueConstraintsFromDBMSSysInfoQuery,
    PostgreSQLGetCheckConstraintsFromDBMSSysInfoQuery,
    PostgreSQLGetIndexesFromDBMSSysInfoQuery,
    PostgreSQLGetTriggersFromDBMSSysInfoQuery,
    PostgreSQLGetForeignKeysFromDBMSSysInfoQuery,
    PostgreSQLGetViewsFromDBMSSysInfoQuery,
    PostgreSQLGetDNDBTDbAttributesRecordQuery,
    PostgreSQLGetDNDBTDbObjectRecordsQuery,
    PostgreSQLGetDNDBTScriptExecutionRecordsQuery>
{
    private int _dbmsVersion;

    public PostgreSQLDbModelFromDBMSProvider(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }

    protected override void BeforeReadDbObjects()
    {
        _dbmsVersion = QueryExecutor.QuerySingleOrDefault<int>(
            new GenericQuery(PostgreSQLQueriesHelper.SelectDbmsVersionStatement));
    }

    protected override void BuildAdditionalDbObjects(Database database)
    {
        PostgreSQLDatabase db = (PostgreSQLDatabase)database;
        db.Sequences = BuildSequences(new PostgreSQLGetSequencesFromDBMSSysInfoQuery());
        db.CompositeTypes = BuildCompositeTypes(new PostgreSQLGetCompositeTypesFromDBMSSysInfoQuery());
        db.DomainTypes = BuildDomainTypes(new PostgreSQLGetDomainTypesFromDBMSSysInfoQuery());
        db.EnumTypes = BuildEnumTypes(new PostgreSQLGetEnumTypesFromDBMSSysInfoQuery());
        db.RangeTypes = BuildRangeTypes(new PostgreSQLGetRangeTypesFromDBMSSysInfoQuery(_dbmsVersion));
        db.Functions = BuildFunctions(new PostgreSQLGetFunctionsFromDBMSSysInfoQuery());
    }

    protected override void ReplaceAdditionalDbModelObjectsIDsAndCodeWithDNDBTSysInfo(Database database, Dictionary<string, DNDBTInfo> dbObjectIDsMap)
    {
        PostgreSQLDatabase db = (PostgreSQLDatabase)database;
        foreach (PostgreSQLSequence sequence in db.Sequences)
            sequence.ID = dbObjectIDsMap[$"{DbObjectType.Sequence}_{sequence.Name}_{null}"].ID;
        foreach (PostgreSQLCompositeType type in db.CompositeTypes)
            type.ID = dbObjectIDsMap[$"{DbObjectType.UserDefinedType}_{type.Name}_{null}"].ID;
        foreach (PostgreSQLDomainType type in db.DomainTypes)
        {
            DNDBTInfo dndbtInfo = dbObjectIDsMap[$"{DbObjectType.UserDefinedType}_{type.Name}_{null}"];
            type.ID = dndbtInfo.ID;
            type.Default.Code = dndbtInfo.Code;

            foreach (CheckConstraint ck in type.CheckConstraints)
            {
                DNDBTInfo dndbtInfoCK = dbObjectIDsMap[$"{DbObjectType.CheckConstraint}_{ck.Name}_{type.ID}"];
                ck.ID = dndbtInfoCK.ID;
                ck.Expression.Code = dndbtInfoCK.Code;
            }
        }
        foreach (PostgreSQLEnumType type in db.EnumTypes)
            type.ID = dbObjectIDsMap[$"{DbObjectType.UserDefinedType}_{type.Name}_{null}"].ID;
        foreach (PostgreSQLRangeType type in db.RangeTypes)
            type.ID = dbObjectIDsMap[$"{DbObjectType.UserDefinedType}_{type.Name}_{null}"].ID;
        foreach (PostgreSQLFunction func in db.Functions)
        {
            DNDBTInfo dndbtInfo = dbObjectIDsMap[$"{DbObjectType.Function}_{func.Name}_{null}"];
            func.ID = dndbtInfo.ID;
            func.CreateStatement.Code = dndbtInfo.Code;
        }
    }

    private List<PostgreSQLSequence> BuildSequences(PostgreSQLGetSequencesFromDBMSSysInfoQuery query)
    {
        IEnumerable<SequenceRecord> sequenceRecordsList = QueryExecutor.Query<SequenceRecord>(query);
        List<PostgreSQLSequence> sequencesList = new();
        foreach (SequenceRecord sequenceRecord in sequenceRecordsList)
        {
            PostgreSQLSequence sequence = new()
            {
                ID = Guid.NewGuid(),
                Name = sequenceRecord.SequenceName,
                DataType = PostgreSQLQueriesHelper.CreateDataTypeModel(sequenceRecord.DataType, "-1", 0),
                Options = new PostgreSQLSequenceOptions()
                {
                    StartWith = sequenceRecord.StartWith,
                    IncrementBy = sequenceRecord.IncrementBy,
                    MinValue = sequenceRecord.MinValue,
                    MaxValue = sequenceRecord.MaxValue,
                    Cache = sequenceRecord.Cache,
                    Cycle = sequenceRecord.Cycle,
                },
                OwnedBy = (sequenceRecord.OwnedByTableName, sequenceRecord.OwnedByColumnName),
            };

            sequencesList.Add(sequence);
        }
        return sequencesList;
    }

    private List<PostgreSQLCompositeType> BuildCompositeTypes(PostgreSQLGetCompositeTypesFromDBMSSysInfoQuery query)
    {
        IEnumerable<CompositeTypeRecord> typeRecordsList = QueryExecutor.Query<CompositeTypeRecord>(query);
        Dictionary<string, PostgreSQLCompositeType> typesMap = new();
        foreach (CompositeTypeRecord typeRecord in typeRecordsList)
        {
            if (!typesMap.ContainsKey(typeRecord.TypeName))
            {
                PostgreSQLCompositeType type = new()
                {
                    ID = Guid.NewGuid(),
                    Name = typeRecord.TypeName,
                };
                typesMap.Add(typeRecord.TypeName, type);
            }

            PostgreSQLCompositeTypeAttribute attribute = new()
            {
                Name = typeRecord.AttributeName,
                DataType = PostgreSQLQueriesHelper.CreateDataTypeModel(
                    typeRecord.AttributeArrayDims > 0 ? typeRecord.AttributeArrayElemDataType : typeRecord.AttributeDataTypeName,
                    typeRecord.AttributeDataTypeLength,
                    typeRecord.AttributeArrayDims),
            };

            typesMap[typeRecord.TypeName].Attributes.Add(attribute);
        }
        return typesMap.Select(x => x.Value).ToList();
    }

    private List<PostgreSQLDomainType> BuildDomainTypes(PostgreSQLGetDomainTypesFromDBMSSysInfoQuery query)
    {
        IEnumerable<DomainTypeRecord> typeRecordsList = QueryExecutor.Query<DomainTypeRecord>(query);
        Dictionary<string, PostgreSQLDomainType> typesMap = new();
        foreach (DomainTypeRecord typeRecord in typeRecordsList)
        {
            if (!typesMap.ContainsKey(typeRecord.TypeName))
            {
                DataType underlyingType = PostgreSQLQueriesHelper.CreateDataTypeModel(
                    typeRecord.UnderlyingTypeArrayDims > 0 ? typeRecord.UnderlyingTypeArrayElemDataType : typeRecord.UnderlyingTypeName,
                    typeRecord.UnderlyingTypeLength,
                    typeRecord.UnderlyingTypeArrayDims);
                PostgreSQLDomainType type = new()
                {
                    ID = Guid.NewGuid(),
                    Name = typeRecord.TypeName,
                    UnderlyingType = underlyingType,
                    NotNull = typeRecord.NotNull,
                    Default = PostgreSQLQueriesHelper.ParseDefault(typeRecord.Default),
                    CheckConstraints = new List<CheckConstraint>(),
                };
                typesMap.Add(typeRecord.TypeName, type);
            }

            if (typeRecord.CheckConstrantName != null)
            {
                CheckConstraint checkConstraint = new()
                {
                    ID = Guid.NewGuid(),
                    Name = typeRecord.CheckConstrantName,
                    Expression = new CodePiece { Code = typeRecord.CheckConstrantDefinition.ParseOutCheckExpression() },
                };
                typesMap[typeRecord.TypeName].CheckConstraints.Add(checkConstraint);
            }
        }
        return typesMap.Select(x => x.Value).ToList();
    }

    private List<PostgreSQLEnumType> BuildEnumTypes(PostgreSQLGetEnumTypesFromDBMSSysInfoQuery query)
    {
        IEnumerable<EnumTypeRecord> typeRecordsList = QueryExecutor.Query<EnumTypeRecord>(query);
        Dictionary<string, PostgreSQLEnumType> typesMap = new();
        foreach (EnumTypeRecord typeRecord in typeRecordsList)
        {
            if (!typesMap.ContainsKey(typeRecord.TypeName))
            {
                PostgreSQLEnumType type = new()
                {
                    ID = Guid.NewGuid(),
                    Name = typeRecord.TypeName,
                };
                typesMap.Add(typeRecord.TypeName, type);
            }

            typesMap[typeRecord.TypeName].AllowedValues.Add(typeRecord.LabelName);
        }
        return typesMap.Select(x => x.Value).ToList();
    }

    private List<PostgreSQLRangeType> BuildRangeTypes(PostgreSQLGetRangeTypesFromDBMSSysInfoQuery query)
    {
        IEnumerable<RangeTypeRecord> typeRecordsList = QueryExecutor.Query<RangeTypeRecord>(query);
        List<PostgreSQLRangeType> typesList = new();
        foreach (RangeTypeRecord typeRecord in typeRecordsList)
        {
            PostgreSQLRangeType type = new()
            {
                ID = Guid.NewGuid(),
                Name = typeRecord.TypeName,
            };

            type.Subtype = PostgreSQLQueriesHelper.CreateDataTypeModel(
                typeRecord.SubtypeArrayElemDataType != null ? typeRecord.SubtypeArrayElemDataType : typeRecord.SubtypeName,
                "-1",
                typeRecord.SubtypeArrayElemDataType != null ? 1 : 0);
            type.SubtypeOperatorClass = typeRecord.SubtypeOperatorClass;
            type.Collation = typeRecord.Collation;
            type.CanonicalFunction = typeRecord.CanonicalFunction == "-" ? null : typeRecord.CanonicalFunction;
            type.SubtypeDiff = typeRecord.SubtypeDiff == "-" ? null : typeRecord.SubtypeDiff;
            type.MultirangeTypeName = typeRecord.MultirangeTypeName;

            typesList.Add(type);
        }
        return typesList;
    }

    private List<PostgreSQLFunction> BuildFunctions(PostgreSQLGetFunctionsFromDBMSSysInfoQuery query)
    {
        IEnumerable<FunctionRecord> funcRecordsList = QueryExecutor.Query<FunctionRecord>(query);
        List<PostgreSQLFunction> funcsList = new();
        foreach (FunctionRecord funcRecord in funcRecordsList)
        {
            PostgreSQLFunction func = new()
            {
                ID = Guid.NewGuid(),
                Name = funcRecord.FunctionName,
                CreateStatement = new CodePiece { Code = funcRecord.FunctionCode },
            };
            funcsList.Add(func);
        }
        return funcsList;
    }
}
