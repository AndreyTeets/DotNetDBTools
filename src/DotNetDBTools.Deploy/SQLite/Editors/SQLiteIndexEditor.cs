using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.SQLite.Editors;

internal class SQLiteIndexEditor : IndexEditor<
    SQLiteInsertDNDBTDbObjectRecordQuery,
    SQLiteDeleteDNDBTDbObjectRecordQuery>
{
    public SQLiteIndexEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }
}
