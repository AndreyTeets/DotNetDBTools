using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MySQL.Editors;

internal class MySQLIndexEditor : IndexEditor<
    MySQLInsertDNDBTDbObjectRecordQuery,
    MySQLDeleteDNDBTDbObjectRecordQuery>
{
    public MySQLIndexEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }
}
