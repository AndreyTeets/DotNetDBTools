using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Deploy.Core.InstanceCreator;

namespace DotNetDBTools.Deploy.Core.Editors;

internal abstract class TableEditor<
    TInsertDNDBTDbObjectRecordQuery,
    TDeleteDNDBTDbObjectRecordQuery,
    TUpdateDNDBTDbObjectRecordQuery,
    TCreateTableQuery,
    TDropTableQuery,
    TAlterTableQuery>
    : ITableEditor
    where TInsertDNDBTDbObjectRecordQuery : InsertDNDBTDbObjectRecordQuery
    where TDeleteDNDBTDbObjectRecordQuery : DeleteDNDBTDbObjectRecordQuery
    where TUpdateDNDBTDbObjectRecordQuery : UpdateDNDBTDbObjectRecordQuery
    where TCreateTableQuery : CreateTableQuery
    where TDropTableQuery : DropTableQuery
    where TAlterTableQuery : AlterTableQuery
{
    protected readonly IQueryExecutor QueryExecutor;

    protected TableEditor(IQueryExecutor queryExecutor)
    {
        QueryExecutor = queryExecutor;
    }

    public void CreateTables(DatabaseDiff dbDiff)
    {
        foreach (Table table in dbDiff.AddedTables)
            CreateTable(table);
    }

    public void DropTables(DatabaseDiff dbDiff)
    {
        foreach (Table table in dbDiff.RemovedTables)
            DropTable(table);
    }

    public void AlterTables(DatabaseDiff dbDiff)
    {
        foreach (TableDiff tableDiff in dbDiff.ChangedTables)
            AlterTable(tableDiff);
    }

    protected virtual void CreateTable(Table table)
    {
        QueryExecutor.Execute(Create<TCreateTableQuery>(table));
        QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(table.ID, null, DbObjectType.Table, table.Name));
        foreach (Column c in table.Columns)
            QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(c.ID, table.ID, DbObjectType.Column, c.Name, c.GetCode()));
        PrimaryKey pk = table.PrimaryKey;
        if (pk is not null)
            QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(pk.ID, table.ID, DbObjectType.PrimaryKey, pk.Name));
        foreach (UniqueConstraint uc in table.UniqueConstraints)
            QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(uc.ID, table.ID, DbObjectType.UniqueConstraint, uc.Name));
        foreach (CheckConstraint ck in table.CheckConstraints)
            QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(ck.ID, table.ID, DbObjectType.CheckConstraint, ck.Name, ck.GetCode()));
    }

    protected virtual void DropTable(Table table)
    {
        QueryExecutor.Execute(Create<TDropTableQuery>(table));
        foreach (CheckConstraint ck in table.CheckConstraints)
            QueryExecutor.Execute(Create<TDeleteDNDBTDbObjectRecordQuery>(ck.ID));
        foreach (UniqueConstraint uc in table.UniqueConstraints)
            QueryExecutor.Execute(Create<TDeleteDNDBTDbObjectRecordQuery>(uc.ID));
        PrimaryKey pk = table.PrimaryKey;
        if (pk is not null)
            QueryExecutor.Execute(Create<TDeleteDNDBTDbObjectRecordQuery>(pk.ID));
        foreach (Column column in table.Columns)
            QueryExecutor.Execute(Create<TDeleteDNDBTDbObjectRecordQuery>(column.ID));
        QueryExecutor.Execute(Create<TDeleteDNDBTDbObjectRecordQuery>(table.ID));
    }

    protected virtual void AlterTable(TableDiff tableDiff)
    {
        QueryExecutor.Execute(Create<TAlterTableQuery>(tableDiff));

        foreach (CheckConstraint ck in tableDiff.CheckConstraintsToDrop)
            QueryExecutor.Execute(Create<TDeleteDNDBTDbObjectRecordQuery>(ck.ID));
        foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToDrop)
            QueryExecutor.Execute(Create<TDeleteDNDBTDbObjectRecordQuery>(uc.ID));
        if (tableDiff.PrimaryKeyToDrop is not null)
            QueryExecutor.Execute(Create<TDeleteDNDBTDbObjectRecordQuery>(tableDiff.PrimaryKeyToDrop.ID));
        foreach (Column column in tableDiff.RemovedColumns)
            QueryExecutor.Execute(Create<TDeleteDNDBTDbObjectRecordQuery>(column.ID));

        QueryExecutor.Execute(Create<TUpdateDNDBTDbObjectRecordQuery>(tableDiff.NewTable.ID, tableDiff.NewTable.Name));
        foreach (ColumnDiff cDiff in tableDiff.ChangedColumns)
            QueryExecutor.Execute(Create<TUpdateDNDBTDbObjectRecordQuery>(cDiff.NewColumn.ID, cDiff.NewColumn.Name, cDiff.NewColumn.GetCode()));

        foreach (Column c in tableDiff.AddedColumns)
            QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(c.ID, tableDiff.NewTable.ID, DbObjectType.Column, c.Name, c.GetCode()));
        PrimaryKey pk = tableDiff.PrimaryKeyToCreate;
        if (pk is not null)
            QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(pk.ID, tableDiff.NewTable.ID, DbObjectType.PrimaryKey, pk.Name));
        foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToCreate)
            QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(uc.ID, tableDiff.NewTable.ID, DbObjectType.UniqueConstraint, uc.Name));
        foreach (CheckConstraint ck in tableDiff.CheckConstraintsToCreate)
            QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(ck.ID, tableDiff.NewTable.ID, DbObjectType.CheckConstraint, ck.Name, ck.GetCode()));
    }
}
