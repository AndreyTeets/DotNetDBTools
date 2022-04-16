using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.SQLite;

internal class SQLiteScriptExecutor : ScriptExecutor<
    SQLiteInsertDNDBTScriptExecutionRecordQuery,
    SQLiteDeleteDNDBTScriptExecutionRecordQuery>
{
    public SQLiteScriptExecutor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }

    protected override bool AppendSemicolon => true;
}
