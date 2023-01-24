using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Deploy.SQLite.Editors;

internal class SQLiteDbEditor : DbEditor<
    SQLiteCheckDNDBTSysTablesExistQuery,
    SQLiteCreateDNDBTSysTablesQuery,
    SQLiteDropDNDBTSysTablesQuery>
{
    private readonly IScriptExecutor _scriptExecutor;
    private readonly ITableEditor _tableEditor;
    private readonly IIndexEditor _indexEditor;
    private readonly ITriggerEditor _triggerEditor;

    public SQLiteDbEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor)
    {
        _scriptExecutor = new SQLiteScriptExecutor(queryExecutor);
        _tableEditor = new SQLiteTableEditor(queryExecutor);
        _indexEditor = new SQLiteIndexEditor(queryExecutor);
        _triggerEditor = new SQLiteTriggerEditor(queryExecutor);
    }

    public override void PopulateDNDBTSysTables(Database database)
    {
        SQLiteDatabase db = (SQLiteDatabase)database;
        foreach (SQLiteTable table in db.Tables)
        {
            QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(table, DbObjectType.Table));
            foreach (Column c in table.Columns)
                QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(c, DbObjectType.Column, c.GetDefault()));
            PrimaryKey pk = table.PrimaryKey;
            if (pk is not null)
                QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(pk, DbObjectType.PrimaryKey));
            foreach (UniqueConstraint uc in table.UniqueConstraints)
                QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(uc, DbObjectType.UniqueConstraint));
            foreach (CheckConstraint ck in table.CheckConstraints)
                QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(ck, DbObjectType.CheckConstraint, ck.GetExpression()));
            foreach (Index idx in table.Indexes)
                QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(idx, DbObjectType.Index));
            foreach (Trigger trg in table.Triggers)
                QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(trg, DbObjectType.Trigger, trg.GetCreateStatement()));
            foreach (ForeignKey fk in table.ForeignKeys)
                QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(fk, DbObjectType.ForeignKey));
        }
        foreach (SQLiteView view in db.Views)
            QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(view, DbObjectType.View, view.GetCreateStatement()));

        foreach (Script script in db.Scripts)
            QueryExecutor.Execute(new SQLiteInsertDNDBTScriptExecutionRecordQuery(script, -1));
        QueryExecutor.Execute(new SQLiteInsertDNDBTDbAttributesRecordQuery(database));
    }

    public override void ApplyDatabaseDiff(DatabaseDiff databaseDiff, DeployOptions options)
    {
        SQLiteDatabaseDiff dbDiff = (SQLiteDatabaseDiff)databaseDiff;
        if (options.NoTransaction)
            ApplyDatabaseDiff(dbDiff);
        else
            QueryExecutor.ExecuteInTransaction(() => ApplyDatabaseDiff(dbDiff));
    }

    private void ApplyDatabaseDiff(SQLiteDatabaseDiff dbDiff)
    {
        _scriptExecutor.DeleteRemovedScriptsExecutionRecords(dbDiff);
        _scriptExecutor.ExecuteScripts(dbDiff, ScriptKind.BeforePublishOnce);

        _triggerEditor.DropTriggers(dbDiff);
        foreach (SQLiteView view in dbDiff.ViewsToDrop)
            DropView(view);
        _indexEditor.DropIndexes(dbDiff);
        _tableEditor.DropTables(dbDiff);
        _tableEditor.AlterTables(dbDiff);
        _tableEditor.CreateTables(dbDiff);
        _indexEditor.CreateIndexes(dbDiff);
        foreach (SQLiteView view in dbDiff.ViewsToCreate)
            CreateView(view);
        _triggerEditor.CreateTriggers(dbDiff);

        _scriptExecutor.ExecuteScripts(dbDiff, ScriptKind.AfterPublishOnce);

        if (dbDiff.NewDatabaseVersion != dbDiff.OldDatabaseVersion)
            QueryExecutor.Execute(new SQLiteUpdateDNDBTDbAttributesRecordQuery(dbDiff.NewDatabaseVersion));
    }

    private void CreateView(SQLiteView view)
    {
        QueryExecutor.Execute(new CreateViewQuery(view));
        QueryExecutor.Execute(new SQLiteInsertDNDBTDbObjectRecordQuery(view, DbObjectType.View, view.GetCreateStatement()));
    }

    private void DropView(SQLiteView view)
    {
        QueryExecutor.Execute(new DropViewQuery(view));
        QueryExecutor.Execute(new SQLiteDeleteDNDBTDbObjectRecordQuery(view.ID));
    }
}
