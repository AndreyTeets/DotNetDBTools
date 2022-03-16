using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MSSQL;

internal class MSSQLScriptExecutor : ScriptExecutor<
    MSSQLInsertDNDBTScriptExecutionRecordQuery,
    MSSQLDeleteDNDBTScriptExecutionRecordQuery>
{
    public MSSQLScriptExecutor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }
}
