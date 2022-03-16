using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.PostgreSQL;

internal class PostgreSQLScriptExecutor : ScriptExecutor<
    PostgreSQLInsertDNDBTScriptExecutionRecordQuery,
    PostgreSQLDeleteDNDBTScriptExecutionRecordQuery>
{
    public PostgreSQLScriptExecutor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }
}
