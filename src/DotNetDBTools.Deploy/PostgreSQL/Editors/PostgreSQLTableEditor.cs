using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.PostgreSQL.Editors;

internal class PostgreSQLTableEditor : TableEditor<
    PostgreSQLInsertDNDBTDbObjectRecordQuery,
    PostgreSQLDeleteDNDBTDbObjectRecordQuery,
    PostgreSQLUpdateDNDBTDbObjectRecordQuery,
    CreateTableQuery,
    AlterTableQuery>
{
    public PostgreSQLTableEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }
}
