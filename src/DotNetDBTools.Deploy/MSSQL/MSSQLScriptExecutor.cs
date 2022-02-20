using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MSSQL.Editors;

internal class MSSQLScriptExecutor : ScriptExecutor<
    MSSQLInsertDNDBTScriptExecutionRecordQuery,
    MSSQLDeleteDNDBTScriptExecutionRecordQuery>
{
    public MSSQLScriptExecutor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }
}
