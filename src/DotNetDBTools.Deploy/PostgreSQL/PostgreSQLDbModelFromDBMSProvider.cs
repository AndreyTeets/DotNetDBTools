using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.PostgreSQL;
using DotNetDBTools.Deploy.Core;
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
    public PostgreSQLDbModelFromDBMSProvider(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }

    protected override void ReplaceAdditionalDbModelObjectsIDsAndCodeWithDNDBTSysInfo(Database database, Dictionary<string, DNDBTInfo> dbObjectIDsMap)
    {
        PostgreSQLDatabase postgresqlDatabase = (PostgreSQLDatabase)database;
        foreach (PostgreSQLCompositeType type in postgresqlDatabase.CompositeTypes)
            type.ID = dbObjectIDsMap[$"{DbObjectType.UserDefinedType}_{type.Name}_{null}"].ID;
        foreach (PostgreSQLDomainType type in postgresqlDatabase.DomainTypes)
        {
            DNDBTInfo dndbtInfo = dbObjectIDsMap[$"{DbObjectType.UserDefinedType}_{type.Name}_{null}"];
            type.ID = dndbtInfo.ID;
            type.Default.Code = dndbtInfo.Code;

            foreach (CheckConstraint ck in type.CheckConstraints)
            {
                DNDBTInfo dndbtInfoCK = dbObjectIDsMap[$"{DbObjectType.CheckConstraint}_{ck.Name}_{type.ID}"];
                ck.ID = dndbtInfoCK.ID;
                ck.CodePiece.Code = dndbtInfoCK.Code;
            }
        }
        foreach (PostgreSQLEnumType type in postgresqlDatabase.EnumTypes)
            type.ID = dbObjectIDsMap[$"{DbObjectType.UserDefinedType}_{type.Name}_{null}"].ID;
        foreach (PostgreSQLRangeType type in postgresqlDatabase.RangeTypes)
            type.ID = dbObjectIDsMap[$"{DbObjectType.UserDefinedType}_{type.Name}_{null}"].ID;
        foreach (PostgreSQLFunction func in postgresqlDatabase.Functions)
        {
            DNDBTInfo dndbtInfo = dbObjectIDsMap[$"{DbObjectType.Function}_{func.Name}_{null}"];
            func.ID = dndbtInfo.ID;
            func.CodePiece.Code = dndbtInfo.Code;
        }
    }

    protected override void BuildAdditionalDbObjects(Database database)
    {
        PostgreSQLDatabase postgresqlDatabase = (PostgreSQLDatabase)database;
        postgresqlDatabase.CompositeTypes = BuildCompositeTypes(new PostgreSQLGetCompositeTypesFromDBMSSysInfoQuery());
        postgresqlDatabase.DomainTypes = BuildDomainTypes(new PostgreSQLGetDomainTypesFromDBMSSysInfoQuery());
        postgresqlDatabase.EnumTypes = BuildEnumTypes(new PostgreSQLGetEnumTypesFromDBMSSysInfoQuery());
        postgresqlDatabase.RangeTypes = BuildRangeTypes(new PostgreSQLGetRangeTypesFromDBMSSysInfoQuery());
        postgresqlDatabase.Functions = BuildFunctions(new PostgreSQLGetFunctionsFromDBMSSysInfoQuery());
        PostgreSQLDependenciesBuilder.BuildDependencies(postgresqlDatabase);
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
                    Attributes = new List<PostgreSQLCompositeTypeAttribute>(),
                };
                typesMap.Add(typeRecord.TypeName, type);
            }

            PostgreSQLCompositeTypeAttribute attribute = new()
            {
                Name = typeRecord.AttributeName,
                DataType = PostgreSQLQueriesHelper.CreateDataTypeModel(
                typeRecord.AttributeDataTypeName,
                typeRecord.AttributeDataTypeLength,
                typeRecord.AttributeDataTypeIsBaseDataType),
            };
            ((List<PostgreSQLCompositeTypeAttribute>)typesMap[typeRecord.TypeName].Attributes).Add(attribute);
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
                    typeRecord.UnderlyingTypeName,
                    typeRecord.UnderlyingTypeLength,
                    typeRecord.UnderlyingTypeIsBaseDataType);
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

            CheckConstraint checkConstraint = new()
            {
                ID = Guid.NewGuid(),
                Name = typeRecord.CheckConstrantName,
                CodePiece = new CodePiece { Code = typeRecord.CheckConstrantCode },
            };
            ((List<CheckConstraint>)typesMap[typeRecord.TypeName].CheckConstraints).Add(checkConstraint);
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
                    AllowedValues = new List<string>(),
                };
                typesMap.Add(typeRecord.TypeName, type);
            }

            ((List<string>)typesMap[typeRecord.TypeName].AllowedValues).Add(typeRecord.LabelName);
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
                typeRecord.SubtypeName,
                "-1",
                typeRecord.SubtypeIsBaseDataType);
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
                CodePiece = new CodePiece { Code = funcRecord.FunctionCode },
            };
            funcsList.Add(func);
        }
        return funcsList;
    }
}
