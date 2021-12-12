using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MSSQL.Queries.DBMSSysInfo;
using DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;
using static DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo.GetAllDbObjectsFromDNDBTSysInfoQuery;
using static DotNetDBTools.Deploy.MSSQL.Queries.DBMSSysInfo.MSSQLGetTypesFromDBMSSysInfoQuery;

namespace DotNetDBTools.Deploy.MSSQL
{
    internal class MSSQLDbModelFromDbSysInfoBuilder : DbModelFromDbSysInfoBuilder<
        MSSQLDatabase,
        MSSQLTable,
        MSSQLGetColumnsFromDBMSSysInfoQuery,
        MSSQLGetPrimaryKeysFromDBMSSysInfoQuery,
        MSSQLGetUniqueConstraintsFromDBMSSysInfoQuery,
        MSSQLGetForeignKeysFromDBMSSysInfoQuery,
        MSSQLGetAllDbObjectsFromDNDBTSysInfoQuery>
    {
        public MSSQLDbModelFromDbSysInfoBuilder(IQueryExecutor queryExecutor)
            : base(queryExecutor) { }

        protected override void ReplaceAdditionalDbModelObjectsIDsWithRecordOnes(Database database, Dictionary<string, DNDBTInfo> dbObjectIDsMap)
        {
            MSSQLDatabase mssqlDatabase = (MSSQLDatabase)database;
            foreach (MSSQLUserDefinedType udt in mssqlDatabase.UserDefinedTypes)
                udt.ID = dbObjectIDsMap[$"{DbObjectsTypes.UserDefinedType}_{udt.Name}_{null}"].ID;
        }

        protected override void BuildAdditionalDbObjects(Database database)
        {
            MSSQLDatabase mssqlDatabase = (MSSQLDatabase)database;
            mssqlDatabase.UserDefinedTypes = BuildUserDefinedTypes(new MSSQLGetTypesFromDBMSSysInfoQuery());
        }

        private List<MSSQLUserDefinedType> BuildUserDefinedTypes(MSSQLGetTypesFromDBMSSysInfoQuery query)
        {
            IEnumerable<UserDefinedTypeRecord> userDefinedTypeRecords = QueryExecutor.Query<UserDefinedTypeRecord>(query);
            List<MSSQLUserDefinedType> userDefinedTypes = new();
            foreach (UserDefinedTypeRecord userDefinedTypeRecord in userDefinedTypeRecords)
            {
                MSSQLUserDefinedType userDefinedType = query.Mapper.MapToMSSQLUserDefinedTypeModel(userDefinedTypeRecord);
                userDefinedTypes.Add(userDefinedType);
            }
            return userDefinedTypes;
        }
    }
}
