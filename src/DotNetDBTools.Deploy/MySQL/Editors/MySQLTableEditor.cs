using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MySQL.Editors;

internal class MySQLTableEditor : TableEditor<
    MySQLInsertDNDBTDbObjectRecordQuery,
    MySQLDeleteDNDBTDbObjectRecordQuery,
    MySQLUpdateDNDBTDbObjectRecordQuery,
    CreateTableQuery,
    AlterTableQuery>
{
    public MySQLTableEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }
}
