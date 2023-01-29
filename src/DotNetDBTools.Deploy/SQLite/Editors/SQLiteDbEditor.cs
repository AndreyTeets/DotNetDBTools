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
    SQLiteDropDNDBTSysTablesQuery,
    SQLiteInsertDNDBTDbObjectRecordQuery,
    SQLiteInsertDNDBTScriptExecutionRecordQuery,
    SQLiteInsertDNDBTDbAttributesRecordQuery>
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
        _indexEditor.DropIndexes(dbDiff);

        foreach (SQLiteView view in dbDiff.ViewsToDrop)
            DropView(view);

        _tableEditor.DropTables(dbDiff);
        _tableEditor.AlterTables(dbDiff);
        _tableEditor.CreateTables(dbDiff);

        foreach (SQLiteView view in dbDiff.ViewsToCreate)
            CreateView(view);

        _indexEditor.CreateIndexes(dbDiff);
        _triggerEditor.CreateTriggers(dbDiff);

        _scriptExecutor.ExecuteScripts(dbDiff, ScriptKind.AfterPublishOnce);

        if (dbDiff.NewVersion != dbDiff.OldVersion)
            QueryExecutor.Execute(new SQLiteUpdateDNDBTDbAttributesRecordQuery(dbDiff.NewVersion));
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
