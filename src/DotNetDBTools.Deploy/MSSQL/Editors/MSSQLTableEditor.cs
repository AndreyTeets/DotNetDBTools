using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MSSQL.Editors;

internal class MSSQLTableEditor : TableEditor<
    MSSQLInsertDNDBTDbObjectRecordQuery,
    MSSQLDeleteDNDBTDbObjectRecordQuery,
    MSSQLUpdateDNDBTDbObjectRecordQuery,
    CreateTableQuery,
    AlterTableQuery>
{
    public MSSQLTableEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }
}
