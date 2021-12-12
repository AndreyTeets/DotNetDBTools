using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.PostgreSQL;

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
    }
}
