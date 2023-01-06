using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MSSQL.Editors;

internal class MSSQLTriggerEditor : TriggerEditor<
    MSSQLInsertDNDBTDbObjectRecordQuery,
    MSSQLDeleteDNDBTDbObjectRecordQuery>
{
    public MSSQLTriggerEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }
}
