﻿using DotNetDBTools.Deploy.Common.Editors;
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
    MySQLDropDNDBTSysTablesQuery,
    MySQLInsertDNDBTDbObjectRecordQuery,
    MySQLInsertDNDBTScriptExecutionRecordQuery,
    MySQLInsertDNDBTDbAttributesRecordQuery>
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

    protected override void PopulateDNDBTSysTablesWithAdditionalObjects(Database database)
    {
        MySQLDatabase db = (MySQLDatabase)database;
        foreach (MySQLFunction func in db.Functions)
            QueryExecutor.Execute(new MySQLInsertDNDBTDbObjectRecordQuery(func, DbObjectType.Function, func.GetCreateStatement()));
        foreach (MySQLProcedure proc in db.Procedures)
            QueryExecutor.Execute(new MySQLInsertDNDBTDbObjectRecordQuery(proc, DbObjectType.Procedure, proc.GetCreateStatement()));
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

        if (dbDiff.NewVersion != dbDiff.OldVersion)
            QueryExecutor.Execute(new MySQLUpdateDNDBTDbAttributesRecordQuery(dbDiff.NewVersion));
    }

    private void CreateFunction(MySQLFunction func)
    {
        QueryExecutor.Execute(new GenericQuery($"{func.GetCreateStatement().AppendSemicolonIfAbsent()}"));
        QueryExecutor.Execute(new MySQLInsertDNDBTDbObjectRecordQuery(func, DbObjectType.Function, func.GetCreateStatement()));
    }

    private void DropFunction(MySQLFunction func)
    {
        QueryExecutor.Execute(new GenericQuery($"DROP FUNCTION `{func.Name}`;"));
        QueryExecutor.Execute(new MySQLDeleteDNDBTDbObjectRecordQuery(func.ID));
    }

    private void CreateView(MySQLView view)
    {
        QueryExecutor.Execute(new CreateViewQuery(view));
        QueryExecutor.Execute(new MySQLInsertDNDBTDbObjectRecordQuery(view, DbObjectType.View, view.GetCreateStatement()));
    }

    private void DropView(MySQLView view)
    {
        QueryExecutor.Execute(new DropViewQuery(view));
        QueryExecutor.Execute(new MySQLDeleteDNDBTDbObjectRecordQuery(view.ID));
    }

    private void CreateProcedure(MySQLProcedure proc)
    {
        QueryExecutor.Execute(new GenericQuery($"{proc.GetCreateStatement().AppendSemicolonIfAbsent()}"));
        QueryExecutor.Execute(new MySQLInsertDNDBTDbObjectRecordQuery(proc, DbObjectType.Procedure, proc.GetCreateStatement()));
    }

    private void DropProcedure(MySQLProcedure proc)
    {
        QueryExecutor.Execute(new GenericQuery($"DROP PROCEDURE `{proc.Name}`;"));
        QueryExecutor.Execute(new MySQLDeleteDNDBTDbObjectRecordQuery(proc.ID));
    }
}
