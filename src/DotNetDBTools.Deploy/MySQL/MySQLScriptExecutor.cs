using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MySQL;

internal class MySQLScriptExecutor : ScriptExecutor<
    MySQLInsertDNDBTScriptExecutionRecordQuery,
    MySQLDeleteDNDBTScriptExecutionRecordQuery>
{
    public MySQLScriptExecutor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }

    protected override bool AppendSemicolon => true;
}
