using System;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Deploy.SQLite.Editors;

internal class SQLiteDbEditor : DbEditor<
    SQLiteCheckDNDBTSysTablesExistQuery,
    SQLiteCreateDNDBTSysTablesQuery,
    SQLiteDropDNDBTSysTablesQuery>
{
    private readonly ITableEditor _tableEditor;

    public SQLiteDbEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor)
    {
        _tableEditor = new SQLiteTableEditor(queryExecutor);
    }

    public override void PopulateDNDBTSysTables(Database database)
    {
        SQLiteDatabase db = (SQLiteDatabase)database;
        foreach (SQLiteTable table in db.Tables)
        {
            QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(table.ID, null, DbObjectType.Table, table.Name));
            foreach (Column c in table.Columns)
                QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(c.ID, table.ID, DbObjectType.Column, c.Name, c.GetCode()));
            PrimaryKey pk = table.PrimaryKey;
            if (pk is not null)
                QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(pk.ID, table.ID, DbObjectType.PrimaryKey, pk.Name));
            foreach (UniqueConstraint uc in table.UniqueConstraints)
                QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(uc.ID, table.ID, DbObjectType.UniqueConstraint, uc.Name));
            foreach (CheckConstraint ck in table.CheckConstraints)
                QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(ck.ID, table.ID, DbObjectType.CheckConstraint, ck.Name, ck.GetCode()));
            foreach (Index index in table.Indexes)
                QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(index.ID, table.ID, DbObjectType.Index, index.Name));
            foreach (Trigger trigger in table.Triggers)
                QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(trigger.ID, table.ID, DbObjectType.Trigger, trigger.Name));
            foreach (ForeignKey fk in table.ForeignKeys)
                QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(fk.ID, table.ID, DbObjectType.ForeignKey, fk.Name));
        }
        foreach (SQLiteView view in db.Views)
            QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(view.ID, null, DbObjectType.View, view.Name, view.GetCode()));
    }

    public override void ApplyDatabaseDiff(DatabaseDiff databaseDiff, DeployOptions options)
    {
        SQLiteDatabaseDiff dbDiff = (SQLiteDatabaseDiff)databaseDiff;
        QueryExecutor.BeginTransaction();
        try
        {
            foreach (SQLiteView view in dbDiff.ViewsToDrop)
                DropView(view);
            _tableEditor.DropTables(dbDiff);
            _tableEditor.AlterTables(dbDiff);
            _tableEditor.CreateTables(dbDiff);
            foreach (SQLiteView view in dbDiff.ViewsToCreate)
                CreateView(view);
        }
        catch (Exception)
        {
            QueryExecutor.RollbackTransaction();
            throw;
        }
        QueryExecutor.CommitTransaction();
    }

    private void CreateView(SQLiteView view)
    {
        QueryExecutor.Execute(new GenericQuery($"{view.GetCode()}"));
        QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(view.ID, null, DbObjectType.View, view.Name, view.GetCode()));
    }

    private void DropView(SQLiteView view)
    {
        QueryExecutor.Execute(new GenericQuery($"DROP VIEW {view.Name};"));
        QueryExecutor.Execute(new SQLiteDeleteDNDBTDbObjectRecordQuery(view.ID));
    }
}
