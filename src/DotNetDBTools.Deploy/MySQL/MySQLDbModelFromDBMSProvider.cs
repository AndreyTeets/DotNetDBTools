using DotNetDBTools.Analysis.MySQL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MySQL.Queries.DBMSSysInfo;
using DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Deploy.MySQL;

internal class MySQLDbModelFromDBMSProvider : DbModelFromDBMSProvider<
    MySQLDatabase,
    MySQLTable,
    MySQLView,
    MySQLGetColumnsFromDBMSSysInfoQuery,
    MySQLGetPrimaryKeysFromDBMSSysInfoQuery,
    MySQLGetUniqueConstraintsFromDBMSSysInfoQuery,
    MySQLGetCheckConstraintsFromDBMSSysInfoQuery,
    MySQLGetIndexesFromDBMSSysInfoQuery,
    MySQLGetTriggersFromDBMSSysInfoQuery,
    MySQLGetForeignKeysFromDBMSSysInfoQuery,
    MySQLGetViewsFromDBMSSysInfoQuery,
    MySQLGetDNDBTDbAttributesRecordQuery,
    MySQLGetDNDBTDbObjectRecordsQuery,
    MySQLGetDNDBTScriptExecutionRecordsQuery>
{
    public MySQLDbModelFromDBMSProvider(IQueryExecutor queryExecutor)
        : base(queryExecutor, new MySQLDbModelPostProcessor()) { }
}
