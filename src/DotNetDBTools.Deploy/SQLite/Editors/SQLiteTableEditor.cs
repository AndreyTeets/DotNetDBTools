using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.SQLite.Queries.DDL;
using DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.SQLite.Editors;

internal class SQLiteTableEditor : TableEditor<
    SQLiteInsertDNDBTDbObjectRecordQuery,
    SQLiteDeleteDNDBTDbObjectRecordQuery,
    SQLiteUpdateDNDBTDbObjectRecordQuery,
    SQLiteCreateTableQuery,
    SQLiteDropTableQuery,
    SQLiteAlterTableQuery>
{
    public SQLiteTableEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }

    protected override void CreateTable(Table table)
    {
        base.CreateTable(table);
        foreach (ForeignKey fk in table.ForeignKeys)
            QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(fk.ID, table.ID, DbObjectType.ForeignKey, fk.Name));
        foreach (Trigger trg in table.Triggers)
            QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(trg.ID, table.ID, DbObjectType.Trigger, trg.Name, trg.GetCode()));
        foreach (Index idx in table.Indexes)
            QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(idx.ID, table.ID, DbObjectType.Index, idx.Name));
    }

    protected override void DropTable(Table table)
    {
        base.DropTable(table);
        foreach (Index idx in table.Indexes)
            QueryExecutor.Execute(new SQLiteDeleteDNDBTDbObjectRecordQuery(idx.ID));
        foreach (Trigger trg in table.Triggers)
            QueryExecutor.Execute(new SQLiteDeleteDNDBTDbObjectRecordQuery(trg.ID));
        foreach (ForeignKey fk in table.ForeignKeys)
            QueryExecutor.Execute(new SQLiteDeleteDNDBTDbObjectRecordQuery(fk.ID));
    }

    protected override void AlterTable(TableDiff tableDiff)
    {
        base.AlterTable(tableDiff);

        foreach (Index idx in tableDiff.IndexesToDrop)
            QueryExecutor.Execute(new SQLiteDeleteDNDBTDbObjectRecordQuery(idx.ID));
        foreach (Trigger trg in tableDiff.TriggersToDrop)
            QueryExecutor.Execute(new SQLiteDeleteDNDBTDbObjectRecordQuery(trg.ID));
        foreach (ForeignKey fk in tableDiff.ForeignKeysToDrop)
            QueryExecutor.Execute(new SQLiteDeleteDNDBTDbObjectRecordQuery(fk.ID));

        foreach (ForeignKey fk in tableDiff.ForeignKeysToCreate)
            QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(fk.ID, tableDiff.NewTable.ID, DbObjectType.ForeignKey, fk.Name));
        foreach (Trigger trg in tableDiff.TriggersToCreate)
            QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(trg.ID, tableDiff.NewTable.ID, DbObjectType.Trigger, trg.Name, trg.GetCode()));
        foreach (Index idx in tableDiff.IndexesToCreate)
            QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(idx.ID, tableDiff.NewTable.ID, DbObjectType.Index, idx.Name));
    }
}
