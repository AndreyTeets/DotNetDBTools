using System;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;

internal class SQLiteDeleteDNDBTScriptExecutionRecordQuery : DeleteDNDBTScriptExecutionRecordQuery
{
    public SQLiteDeleteDNDBTScriptExecutionRecordQuery(Guid scriptID)
        : base(scriptID) { }

    protected override string GetSql(Guid scriptID)
    {
        string query =
$@"DELETE FROM [{DNDBTSysTables.DNDBTScriptExecutions}]
WHERE [{DNDBTSysTables.DNDBTScriptExecutions.ID}] = '{scriptID}';";

        return query;
    }
}
