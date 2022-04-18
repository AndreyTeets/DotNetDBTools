using DotNetDBTools.Analysis.MySQL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MySQL.Queries.DBMSSysInfo;
using DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
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

    protected override void BuildAdditionalDbObjects(Database database)
    {
        MySQLDatabase db = (MySQLDatabase)database;
        db.Functions = new();
        db.Procedures = new();
    }
}
