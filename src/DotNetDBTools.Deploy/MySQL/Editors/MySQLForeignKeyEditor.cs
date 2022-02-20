using DotNetDBTools.Deploy.Common.Editors;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MySQL.Queries.DDL;
using DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MySQL.Editors;

internal class MySQLForeignKeyEditor : ForeignKeyEditor<
    MySQLInsertDNDBTDbObjectRecordQuery,
    MySQLDeleteDNDBTDbObjectRecordQuery,
    MySQLCreateForeignKeyQuery,
    MySQLDropForeignKeyQuery>
{
    public MySQLForeignKeyEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }
}
