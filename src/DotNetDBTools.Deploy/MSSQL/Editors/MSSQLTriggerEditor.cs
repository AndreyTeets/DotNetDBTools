using DotNetDBTools.Deploy.Common.Editors;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MSSQL.Queries.DDL;
using DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MSSQL.Editors;

internal class MSSQLTriggerEditor : TriggerEditor<
    MSSQLInsertDNDBTDbObjectRecordQuery,
    MSSQLDeleteDNDBTDbObjectRecordQuery,
    MSSQLCreateTriggerQuery,
    MSSQLDropTriggerQuery>
{
    public MSSQLTriggerEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }
}
