using DotNetDBTools.Deploy.Common.Editors;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Deploy.MSSQL.Queries.DDL;
using DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Generation.MSSQL;
using DotNetDBTools.Models;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;
using DotNetDBTools.Models.MSSQL.UserDefinedTypes;

namespace DotNetDBTools.Deploy.MSSQL.Editors;

internal class MSSQLDbEditor : DbEditor<
    MSSQLCheckDNDBTSysTablesExistQuery,
    MSSQLCreateDNDBTSysTablesQuery,
    MSSQLDropDNDBTSysTablesQuery,
    MSSQLInsertDNDBTDbObjectRecordQuery,
    MSSQLInsertDNDBTScriptExecutionRecordQuery,
    MSSQLInsertDNDBTDbAttributesRecordQuery>
{
    private readonly IScriptExecutor _scriptExecutor;
    private readonly ITableEditor _tableEditor;
    private readonly IIndexEditor _indexEditor;
    private readonly ITriggerEditor _triggerEditor;
    private readonly IForeignKeyEditor _foreignKeyEditor;

    public MSSQLDbEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor)
    {
        _scriptExecutor = new MSSQLScriptExecutor(queryExecutor);
        _tableEditor = new MSSQLTableEditor(queryExecutor);
        _indexEditor = new MSSQLIndexEditor(queryExecutor);
        _triggerEditor = new MSSQLTriggerEditor(queryExecutor);
        _foreignKeyEditor = new MSSQLForeignKeyEditor(queryExecutor);
    }

    protected override void PopulateDNDBTSysTablesWithAdditionalObjects(Database database)
    {
        MSSQLDatabase db = (MSSQLDatabase)database;
        foreach (MSSQLUserDefinedType udt in db.UserDefinedTypes)
            QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(udt, DbObjectType.UserDefinedType));
        foreach (MSSQLUserDefinedTableType udtt in db.UserDefinedTableTypes)
            QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(udtt, DbObjectType.UserDefinedTableType));

        foreach (MSSQLFunction func in db.Functions)
            QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(func, DbObjectType.Function, func.GetCreateStatement()));
        foreach (MSSQLProcedure proc in db.Procedures)
            QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(proc, DbObjectType.Procedure, proc.GetCreateStatement()));
    }

    public override void ApplyDatabaseDiff(DatabaseDiff databaseDiff, DeployOptions options)
    {
        MSSQLDatabaseDiff dbDiff = (MSSQLDatabaseDiff)databaseDiff;
        if (options.NoTransaction)
            ApplyDatabaseDiff(dbDiff);
        else
            QueryExecutor.ExecuteInTransaction(() => ApplyDatabaseDiff(dbDiff));
    }

    private void ApplyDatabaseDiff(MSSQLDatabaseDiff dbDiff)
    {
        _scriptExecutor.DeleteRemovedScriptsExecutionRecords(dbDiff);
        _scriptExecutor.ExecuteScripts(dbDiff, ScriptKind.BeforePublishOnce);

        _triggerEditor.DropTriggers(dbDiff);
        _foreignKeyEditor.DropForeignKeys(dbDiff);
        _indexEditor.DropIndexes(dbDiff);

        foreach (MSSQLProcedure procedure in dbDiff.ProceduresToDrop)
            DropProcedure(procedure);
        foreach (MSSQLFunction function in dbDiff.FunctionsToDrop)
            DropFunction(function);
        foreach (MSSQLView view in dbDiff.ViewsToDrop)
            DropView(view);
        foreach (MSSQLUserDefinedTableType udtt in dbDiff.UserDefinedTableTypesToDrop)
            DropUserDefinedTableType(udtt);

        _tableEditor.DropTables(dbDiff);

        foreach (MSSQLUserDefinedType udt in dbDiff.UserDefinedTypesToDrop)
            RenameUserDefinedTypeToTemp(udt);
        foreach (MSSQLUserDefinedType udt in dbDiff.UserDefinedTypesToCreate)
            CreateUserDefinedType(udt);

        _tableEditor.AlterTables(dbDiff);
        foreach (MSSQLUserDefinedType udt in dbDiff.UserDefinedTypesToDrop)
            Drop_RenamedToTemp_UserDefinedType(udt);

        _tableEditor.CreateTables(dbDiff);

        foreach (MSSQLUserDefinedTableType udtt in dbDiff.UserDefinedTableTypesToCreate)
            CreateUserDefinedTableType(udtt);
        foreach (MSSQLView view in dbDiff.ViewsToCreate)
            CreateView(view);
        foreach (MSSQLFunction function in dbDiff.FunctionsToCreate)
            CreateFunction(function);
        foreach (MSSQLProcedure procedure in dbDiff.ProceduresToCreate)
            CreateProcedure(procedure);

        _indexEditor.CreateIndexes(dbDiff);
        _foreignKeyEditor.CreateForeignKeys(dbDiff);
        _triggerEditor.CreateTriggers(dbDiff);

        _scriptExecutor.ExecuteScripts(dbDiff, ScriptKind.AfterPublishOnce);

        if (dbDiff.NewVersion != dbDiff.OldVersion)
            QueryExecutor.Execute(new MSSQLUpdateDNDBTDbAttributesRecordQuery(dbDiff.NewVersion));
    }

    private void RenameUserDefinedTypeToTemp(MSSQLUserDefinedType udt)
    {
        QueryExecutor.Execute(new MSSQLRenameUserDefinedDataTypeQuery(udt));
        QueryExecutor.Execute(new MSSQLDeleteDNDBTDbObjectRecordQuery(udt.ID));
    }

    private void CreateUserDefinedType(MSSQLUserDefinedType udt)
    {
        QueryExecutor.Execute(new MSSQLCreateTypeQuery(udt));
        QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(udt, DbObjectType.UserDefinedType));
    }

    private void Drop_RenamedToTemp_UserDefinedType(MSSQLUserDefinedType udt)
    {
        MSSQLUserDefinedType renamedUdt = udt.CopyModel();
        renamedUdt.Name = $"_DNDBTTemp_{udt.Name}";
        QueryExecutor.Execute(new MSSQLDropTypeQuery(renamedUdt));
    }

    private void CreateUserDefinedTableType(MSSQLUserDefinedTableType udtt)
    {
        //QueryExecutor.Execute(new CreateTableQuery(udtt));
        QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(udtt, DbObjectType.UserDefinedTableType));
    }

    private void DropUserDefinedTableType(MSSQLUserDefinedTableType udtt)
    {
        //QueryExecutor.Execute(new DropTableQuery(udtt));
        QueryExecutor.Execute(new MSSQLDeleteDNDBTDbObjectRecordQuery(udtt.ID));
    }

    private void CreateFunction(MSSQLFunction func)
    {
        QueryExecutor.Execute(new GenericQuery($"{func.GetCreateStatement()}"));
        QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(func, DbObjectType.Function, func.GetCreateStatement()));
    }

    private void DropFunction(MSSQLFunction func)
    {
        QueryExecutor.Execute(new GenericQuery($"DROP FUNCTION [{func.Name}];"));
        QueryExecutor.Execute(new MSSQLDeleteDNDBTDbObjectRecordQuery(func.ID));
    }

    private void CreateView(MSSQLView view)
    {
        QueryExecutor.Execute(new CreateViewQuery(view));
        QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(view, DbObjectType.View, view.GetCreateStatement()));
    }

    private void DropView(MSSQLView view)
    {
        QueryExecutor.Execute(new DropViewQuery(view));
        QueryExecutor.Execute(new MSSQLDeleteDNDBTDbObjectRecordQuery(view.ID));
    }

    private void CreateProcedure(MSSQLProcedure proc)
    {
        QueryExecutor.Execute(new GenericQuery($"{proc.GetCreateStatement()}"));
        QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(proc, DbObjectType.Procedure, proc.GetCreateStatement()));
    }

    private void DropProcedure(MSSQLProcedure proc)
    {
        QueryExecutor.Execute(new GenericQuery($"DROP PROCEDURE [{proc.Name}];"));
        QueryExecutor.Execute(new MSSQLDeleteDNDBTDbObjectRecordQuery(proc.ID));
    }
}
