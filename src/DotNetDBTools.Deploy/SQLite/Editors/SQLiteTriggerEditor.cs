using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.SQLite.Editors;

internal class SQLiteTriggerEditor : TriggerEditor<
    SQLiteInsertDNDBTDbObjectRecordQuery,
    SQLiteDeleteDNDBTDbObjectRecordQuery>
{
    public SQLiteTriggerEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }
}
