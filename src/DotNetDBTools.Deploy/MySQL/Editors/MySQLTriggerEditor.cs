using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MySQL.Editors;

internal class MySQLTriggerEditor : TriggerEditor<
    MySQLInsertDNDBTDbObjectRecordQuery,
    MySQLDeleteDNDBTDbObjectRecordQuery>
{
    public MySQLTriggerEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }
}
