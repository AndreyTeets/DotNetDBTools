using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MySQL.Editors;

internal class MySQLScriptExecutor : ScriptExecutor<
    MySQLInsertDNDBTScriptExecutionRecordQuery,
    MySQLDeleteDNDBTScriptExecutionRecordQuery>
{
    public MySQLScriptExecutor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }
}
