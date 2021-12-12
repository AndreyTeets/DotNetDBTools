using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MySQL.Queries.DBMSSysInfo;
using DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Deploy.MySQL
{
    internal class MySQLDbModelFromDbSysInfoBuilder : DbModelFromDbSysInfoBuilder<
        MySQLDatabase,
        MySQLTable,
        MySQLGetColumnsFromDBMSSysInfoQuery,
        MySQLGetPrimaryKeysFromDBMSSysInfoQuery,
        MySQLGetUniqueConstraintsFromDBMSSysInfoQuery,
        MySQLGetForeignKeysFromDBMSSysInfoQuery,
        MySQLGetAllDbObjectsFromDNDBTSysInfoQuery>
    {
        public MySQLDbModelFromDbSysInfoBuilder(IQueryExecutor queryExecutor)
            : base(queryExecutor) { }
    }
}
