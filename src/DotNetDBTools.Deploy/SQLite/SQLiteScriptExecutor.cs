using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.SQLite.Editors;

internal class SQLiteScriptExecutor : ScriptExecutor<
    SQLiteInsertDNDBTScriptExecutionRecordQuery,
    SQLiteDeleteDNDBTScriptExecutionRecordQuery>
{
    public SQLiteScriptExecutor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }
}
