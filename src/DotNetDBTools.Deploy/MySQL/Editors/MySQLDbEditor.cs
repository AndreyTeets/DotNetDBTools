using DotNetDBTools.Deploy.Common.Editors;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Generation.MySQL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Deploy.MySQL.Editors;

internal class MySQLDbEditor : DbEditor<
    MySQLCheckDNDBTSysTablesExistQuery,
    MySQLCreateDNDBTSysTablesQuery,
    MySQLDropDNDBTSysTablesQuery>
{
    private readonly IScriptExecutor _scriptExecutor;
    private readonly ITableEditor _tableEditor;
    private readonly IIndexEditor _indexEditor;
    private readonly ITriggerEditor _triggerEditor;
    private readonly IForeignKeyEditor _foreignKeyEditor;

    public MySQLDbEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor)
    {
        _scriptExecutor = new MySQLScriptExecutor(queryExecutor);
        _tableEditor = new MySQLTableEditor(queryExecutor);
        _indexEditor = new MySQLIndexEditor(queryExecutor);
        _triggerEditor = new MySQLTriggerEditor(queryExecutor);
        _foreignKeyEditor = new MySQLForeignKeyEditor(queryExecutor);
    }

    public override void PopulateDNDBTSysTables(Database database)
    {
        MySQLDatabase db = (MySQLDatabase)database;
        foreach (MySQLTable table in db.Tables)
        {
            QueryExecutor.Execute(new MySQLInsertDNDBTDbObjectRecordQuery(table.ID, null, DbObjectType.Table, table.Name));
            foreach (Column c in table.Columns)
                QueryExecutor.Execute(new MySQLInsertDNDBTDbObjectRecordQuery(c.ID, table.ID, DbObjectType.Column, c.Name, c.GetDefault()));
            PrimaryKey pk = table.PrimaryKey;
            if (pk is not null)
                QueryExecutor.Execute(new MySQLInsertDNDBTDbObjectRecordQuery(pk.ID, table.ID, DbObjectType.PrimaryKey, pk.Name));
            foreach (UniqueConstraint uc in table.UniqueConstraints)
                QueryExecutor.Execute(new MySQLInsertDNDBTDbObjectRecordQuery(uc.ID, table.ID, DbObjectType.UniqueConstraint, uc.Name));
            foreach (CheckConstraint ck in table.CheckConstraints)
                QueryExecutor.Execute(new MySQLInsertDNDBTDbObjectRecordQuery(ck.ID, table.ID, DbObjectType.CheckConstraint, ck.Name, ck.GetExpression()));
            foreach (Index idx in table.Indexes)
                QueryExecutor.Execute(new MySQLInsertDNDBTDbObjectRecordQuery(idx.ID, table.ID, DbObjectType.Index, idx.Name));
            foreach (Trigger trg in table.Triggers)
                QueryExecutor.Execute(new MySQLInsertDNDBTDbObjectRecordQuery(trg.ID, table.ID, DbObjectType.Trigger, trg.Name, trg.GetCreateStatement()));
            foreach (ForeignKey fk in table.ForeignKeys)
                QueryExecutor.Execute(new MySQLInsertDNDBTDbObjectRecordQuery(fk.ID, table.ID, DbObjectType.ForeignKey, fk.Name));
        }

        foreach (MySQLView view in db.Views)
            QueryExecutor.Execute(new MySQLInsertDNDBTDbObjectRecordQuery(view.ID, null, DbObjectType.View, view.Name, view.GetCreateStatement()));
        foreach (MySQLFunction func in db.Functions)
            QueryExecutor.Execute(new MySQLInsertDNDBTDbObjectRecordQuery(func.ID, null, DbObjectType.Function, func.Name, func.GetCreateStatement()));
        foreach (MySQLProcedure proc in db.Procedures)
            QueryExecutor.Execute(new MySQLInsertDNDBTDbObjectRecordQuery(proc.ID, null, DbObjectType.Procedure, proc.Name, proc.GetCreateStatement()));

        foreach (Script script in db.Scripts)
            QueryExecutor.Execute(new MySQLInsertDNDBTScriptExecutionRecordQuery(script, -1));
        QueryExecutor.Execute(new MySQLInsertDNDBTDbAttributesRecordQuery(database));
    }

    public override void ApplyDatabaseDiff(DatabaseDiff databaseDiff, DeployOptions options)
    {
        MySQLDatabaseDiff dbDiff = (MySQLDatabaseDiff)databaseDiff;
        ApplyDatabaseDiff(dbDiff);
    }

    private void ApplyDatabaseDiff(MySQLDatabaseDiff dbDiff)
    {
        _scriptExecutor.DeleteRemovedScriptsExecutionRecords(dbDiff);
        _scriptExecutor.ExecuteScripts(dbDiff, ScriptKind.BeforePublishOnce);

        _triggerEditor.DropTriggers(dbDiff);
        _foreignKeyEditor.DropForeignKeys(dbDiff);
        _indexEditor.DropIndexes(dbDiff);

        foreach (MySQLProcedure procedure in dbDiff.ProceduresToDrop)
            DropProcedure(procedure);
        foreach (MySQLFunction function in dbDiff.FunctionsToDrop)
            DropFunction(function);
        foreach (MySQLView view in dbDiff.ViewsToDrop)
            DropView(view);

        _tableEditor.DropTables(dbDiff);
        _tableEditor.AlterTables(dbDiff);
        _tableEditor.CreateTables(dbDiff);

        foreach (MySQLView view in dbDiff.ViewsToCreate)
            CreateView(view);
        foreach (MySQLFunction function in dbDiff.FunctionsToCreate)
            CreateFunction(function);
        foreach (MySQLProcedure procedure in dbDiff.ProceduresToCreate)
            CreateProcedure(procedure);

        _indexEditor.CreateIndexes(dbDiff);
        _foreignKeyEditor.CreateForeignKeys(dbDiff);
        _triggerEditor.CreateTriggers(dbDiff);

        _scriptExecutor.ExecuteScripts(dbDiff, ScriptKind.AfterPublishOnce);

        if (dbDiff.NewDatabaseVersion != dbDiff.OldDatabaseVersion)
            QueryExecutor.Execute(new MySQLUpdateDNDBTDbAttributesRecordQuery(dbDiff.NewDatabaseVersion));
    }

    private void CreateFunction(MySQLFunction func)
    {
        QueryExecutor.Execute(new GenericQuery($"{func.GetCreateStatement().AppendSemicolonIfAbsent()}"));
        QueryExecutor.Execute(new MySQLInsertDNDBTDbObjectRecordQuery(func.ID, null, DbObjectType.Function, func.Name, func.GetCreateStatement()));
    }

    private void DropFunction(MySQLFunction func)
    {
        QueryExecutor.Execute(new GenericQuery($"DROP FUNCTION `{func.Name}`;"));
        QueryExecutor.Execute(new MySQLDeleteDNDBTDbObjectRecordQuery(func.ID));
    }

    private void CreateView(MySQLView view)
    {
        QueryExecutor.Execute(new CreateViewQuery(view));
        QueryExecutor.Execute(new MySQLInsertDNDBTDbObjectRecordQuery(view.ID, null, DbObjectType.View, view.Name, view.GetCreateStatement()));
    }

    private void DropView(MySQLView view)
    {
        QueryExecutor.Execute(new DropViewQuery(view));
        QueryExecutor.Execute(new MySQLDeleteDNDBTDbObjectRecordQuery(view.ID));
    }

    private void CreateProcedure(MySQLProcedure proc)
    {
        QueryExecutor.Execute(new GenericQuery($"{proc.GetCreateStatement().AppendSemicolonIfAbsent()}"));
        QueryExecutor.Execute(new MySQLInsertDNDBTDbObjectRecordQuery(proc.ID, null, DbObjectType.Procedure, proc.Name, proc.GetCreateStatement()));
    }

    private void DropProcedure(MySQLProcedure proc)
    {
        QueryExecutor.Execute(new GenericQuery($"DROP PROCEDURE `{proc.Name}`;"));
        QueryExecutor.Execute(new MySQLDeleteDNDBTDbObjectRecordQuery(proc.ID));
    }
}
