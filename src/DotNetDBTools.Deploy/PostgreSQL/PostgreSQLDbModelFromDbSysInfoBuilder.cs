using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;
using static DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo.GetAllDbObjectsFromDNDBTSysInfoQuery;
using static DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo.PostgreSQLGetCompositeTypesFromDBMSSysInfoQuery;
using static DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo.PostgreSQLGetDomainTypesFromDBMSSysInfoQuery;
using static DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo.PostgreSQLGetEnumTypesFromDBMSSysInfoQuery;
using static DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo.PostgreSQLGetRangeTypesFromDBMSSysInfoQuery;

namespace DotNetDBTools.Deploy.PostgreSQL
{
    internal class PostgreSQLDbModelFromDbSysInfoBuilder : DbModelFromDbSysInfoBuilder<
        PostgreSQLDatabase,
        PostgreSQLTable,
        PostgreSQLGetColumnsFromDBMSSysInfoQuery,
        PostgreSQLGetPrimaryKeysFromDBMSSysInfoQuery,
        PostgreSQLGetUniqueConstraintsFromDBMSSysInfoQuery,
        PostgreSQLGetForeignKeysFromDBMSSysInfoQuery,
        PostgreSQLGetAllDbObjectsFromDNDBTSysInfoQuery>
    {
        public PostgreSQLDbModelFromDbSysInfoBuilder(IQueryExecutor queryExecutor)
            : base(queryExecutor) { }

        protected override void ReplaceAdditionalDbModelObjectsIDsWithRecordOnes(Database database, Dictionary<string, DNDBTInfo> dbObjectIDsMap)
        {
            PostgreSQLDatabase postgresqlDatabase = (PostgreSQLDatabase)database;
            foreach (PostgreSQLCompositeType type in postgresqlDatabase.CompositeTypes)
                type.ID = dbObjectIDsMap[$"{DbObjectsTypes.UserDefinedType}_{type.Name}_{null}"].ID;
            foreach (PostgreSQLDomainType type in postgresqlDatabase.DomainTypes)
            {
                DNDBTInfo dndbtInfo = dbObjectIDsMap[$"{DbObjectsTypes.UserDefinedType}_{type.Name}_{null}"];
                type.ID = dndbtInfo.ID;
                if (type.Default is DefaultValueAsFunction defaultValueAsFunction)
                    defaultValueAsFunction.FunctionText = dndbtInfo.ExtraInfo;

                foreach (CheckConstraint ck in type.CheckConstraints)
                {
                    DNDBTInfo dndbtInfoCK = dbObjectIDsMap[$"{DbObjectsTypes.CheckConstraint}_{ck.Name}_{type.ID}"];
                    ck.ID = dndbtInfoCK.ID;
                    ck.Code = dndbtInfoCK.ExtraInfo;
                }
            }
            foreach (PostgreSQLEnumType type in postgresqlDatabase.EnumTypes)
                type.ID = dbObjectIDsMap[$"{DbObjectsTypes.UserDefinedType}_{type.Name}_{null}"].ID;
            foreach (PostgreSQLRangeType type in postgresqlDatabase.RangeTypes)
                type.ID = dbObjectIDsMap[$"{DbObjectsTypes.UserDefinedType}_{type.Name}_{null}"].ID;
        }

        protected override void BuildAdditionalDbObjects(Database database)
        {
            PostgreSQLDatabase postgresqlDatabase = (PostgreSQLDatabase)database;
            postgresqlDatabase.CompositeTypes = BuildCompositeTypes(new PostgreSQLGetCompositeTypesFromDBMSSysInfoQuery());
            postgresqlDatabase.DomainTypes = BuildDomainTypes(new PostgreSQLGetDomainTypesFromDBMSSysInfoQuery());
            postgresqlDatabase.EnumTypes = BuildEnumTypes(new PostgreSQLGetEnumTypesFromDBMSSysInfoQuery());
            postgresqlDatabase.RangeTypes = BuildRangeTypes(new PostgreSQLGetRangeTypesFromDBMSSysInfoQuery());
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
                    PostgreSQLDomainType type = new()
                    {
                        ID = Guid.NewGuid(),
                        Name = typeRecord.TypeName,
                        UnderlyingType = PostgreSQLQueriesHelper.CreateDataTypeModel(
                            typeRecord.UnderlyingTypeName,
                            typeRecord.UnderlyingTypeLength,
                            typeRecord.UnderlyingTypeIsBaseDataType),
                        Default = PostgreSQLQueriesHelper.ParseDefault(typeRecord.Default),
                        Nullable = typeRecord.Nullable,
                        CheckConstraints = new List<CheckConstraint>(),
                    };
                    typesMap.Add(typeRecord.TypeName, type);
                }

                CheckConstraint checkConstraint = new()
                {
                    ID = Guid.NewGuid(),
                    Name = typeRecord.CheckConstrantName,
                    Code = typeRecord.CheckConstrantCode
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
    }
}
