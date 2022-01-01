using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MySQL.Queries.DBMSSysInfo;
using DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Deploy.MySQL
{
    internal class MySQLDbModelFromDbSysInfoBuilder : DbModelFromDbSysInfoBuilder<
        MySQLDatabase,
        MySQLTable,
        MySQLView,
        MySQLGetColumnsFromDBMSSysInfoQuery,
        MySQLGetPrimaryKeysFromDBMSSysInfoQuery,
        MySQLGetUniqueConstraintsFromDBMSSysInfoQuery,
        MySQLGetCheckConstraintsFromDBMSSysInfoQuery,
        MySQLGetForeignKeysFromDBMSSysInfoQuery,
        MySQLGetViewsFromDBMSSysInfoQuery,
        MySQLGetAllDbObjectsFromDNDBTSysInfoQuery>
    {
        public MySQLDbModelFromDbSysInfoBuilder(IQueryExecutor queryExecutor)
            : base(queryExecutor) { }
    }
}
