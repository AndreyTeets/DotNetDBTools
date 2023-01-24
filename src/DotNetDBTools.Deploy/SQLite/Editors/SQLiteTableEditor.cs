using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;
using DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.SQLite.Editors;

internal class SQLiteTableEditor : TableEditor<
    SQLiteInsertDNDBTDbObjectRecordQuery,
    SQLiteDeleteDNDBTDbObjectRecordQuery,
    SQLiteUpdateDNDBTDbObjectRecordQuery,
    SQLiteCreateTableQuery,
    SQLiteAlterTableQuery>
{
    public SQLiteTableEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }

    protected override void CreateTable(Table table)
    {
        base.CreateTable(table);
        foreach (ForeignKey fk in table.ForeignKeys)
            QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(fk, DbObjectType.ForeignKey));
    }

    protected override void DropTable(Table table)
    {
        base.DropTable(table);
        foreach (ForeignKey fk in table.ForeignKeys)
            QueryExecutor.Execute(new SQLiteDeleteDNDBTDbObjectRecordQuery(fk.ID));
    }

    protected override void AlterTable(TableDiff tableDiff)
    {
        base.AlterTable(tableDiff);

        foreach (ForeignKey fk in tableDiff.ForeignKeysToDrop)
            QueryExecutor.Execute(new SQLiteDeleteDNDBTDbObjectRecordQuery(fk.ID));

        foreach (ForeignKey fk in tableDiff.ForeignKeysToCreate)
            QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(fk, DbObjectType.ForeignKey));
    }
}
