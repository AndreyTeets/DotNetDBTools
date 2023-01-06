using System.Linq;
using DotNetDBTools.Deploy.Common.Editors;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Deploy.MSSQL.Queries.DDL;
using DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;
using DotNetDBTools.Models.MSSQL.UserDefinedTypes;

namespace DotNetDBTools.Deploy.MSSQL.Editors;

internal class MSSQLDbEditor : DbEditor<
    MSSQLCheckDNDBTSysTablesExistQuery,
    MSSQLCreateDNDBTSysTablesQuery,
    MSSQLDropDNDBTSysTablesQuery>
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

    public override void PopulateDNDBTSysTables(Database database)
    {
        MSSQLDatabase db = (MSSQLDatabase)database;
        foreach (MSSQLUserDefinedType udt in db.UserDefinedTypes)
            QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(udt.ID, null, DbObjectType.UserDefinedType, udt.Name));
        foreach (MSSQLTable table in db.Tables)
        {
            QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(table.ID, null, DbObjectType.Table, table.Name));
            foreach (Column c in table.Columns)
                QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(c.ID, table.ID, DbObjectType.Column, c.Name, c.GetCode()));
            PrimaryKey pk = table.PrimaryKey;
            if (pk is not null)
                QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(pk.ID, table.ID, DbObjectType.PrimaryKey, pk.Name));
            foreach (UniqueConstraint uc in table.UniqueConstraints)
                QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(uc.ID, table.ID, DbObjectType.UniqueConstraint, uc.Name));
            foreach (CheckConstraint ck in table.CheckConstraints)
                QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(ck.ID, table.ID, DbObjectType.CheckConstraint, ck.Name, ck.GetCode()));
            foreach (Index idx in table.Indexes)
                QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(idx.ID, table.ID, DbObjectType.Index, idx.Name));
            foreach (Trigger trg in table.Triggers)
                QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(trg.ID, table.ID, DbObjectType.Trigger, trg.Name, trg.GetCode()));
            foreach (ForeignKey fk in table.ForeignKeys)
                QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(fk.ID, table.ID, DbObjectType.ForeignKey, fk.Name));
        }
        foreach (MSSQLUserDefinedTableType udtt in db.UserDefinedTableTypes)
            QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(udtt.ID, null, DbObjectType.UserDefinedTableType, udtt.Name));
        foreach (MSSQLFunction func in db.Functions)
            QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(func.ID, null, DbObjectType.Function, func.Name, func.GetCode()));
        foreach (MSSQLView view in db.Views)
            QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(view.ID, null, DbObjectType.View, view.Name, view.GetCode()));
        foreach (MSSQLProcedure proc in db.Procedures)
            QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(proc.ID, null, DbObjectType.Procedure, proc.Name, proc.GetCode()));

        foreach (Script script in db.Scripts)
            QueryExecutor.Execute(new MSSQLInsertDNDBTScriptExecutionRecordQuery(script, -1));
        QueryExecutor.Execute(new MSSQLInsertDNDBTDbAttributesRecordQuery(database));
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
        foreach (MSSQLProcedure procedure in dbDiff.ProceduresToDrop)
            DropProcedure(procedure);
        foreach (MSSQLView view in dbDiff.ViewsToDrop)
            DropView(view);
        foreach (MSSQLFunction function in dbDiff.FunctionsToDrop)
            DropFunction(function);
        foreach (MSSQLUserDefinedTableType udtt in dbDiff.UserDefinedTableTypesToDrop)
            DropUserDefinedTableType(udtt);
        _foreignKeyEditor.DropForeignKeys(dbDiff);
        _indexEditor.DropIndexes(dbDiff);
        _tableEditor.DropTables(dbDiff);

        foreach (MSSQLUserDefinedType udt in dbDiff.RemovedUserDefinedTypes.Concat(dbDiff.ChangedUserDefinedTypes.Select(x => x.OldUserDefinedType)))
            RenameUserDefinedTypeToTempInDbAndInDbDiff(udt);
        foreach (MSSQLUserDefinedType udt in dbDiff.AddedUserDefinedTypes.Concat(dbDiff.ChangedUserDefinedTypes.Select(x => x.NewUserDefinedType)))
            CreateUserDefinedType(udt);
        foreach (MSSQLUserDefinedTypeDiff udtDiff in dbDiff.ChangedUserDefinedTypes)
            UseNewUDTInAllTables(udtDiff);
        _tableEditor.AlterTables(dbDiff);
        foreach (MSSQLUserDefinedType udt in dbDiff.RemovedUserDefinedTypes.Concat(dbDiff.ChangedUserDefinedTypes.Select(x => x.OldUserDefinedType)))
            DropUserDefinedType(udt);

        _tableEditor.CreateTables(dbDiff);
        _indexEditor.CreateIndexes(dbDiff);
        _foreignKeyEditor.CreateForeignKeys(dbDiff);
        foreach (MSSQLUserDefinedTableType udtt in dbDiff.UserDefinedTableTypesToCreate)
            CreateUserDefinedTableType(udtt);
        foreach (MSSQLFunction function in dbDiff.FunctionsToCreate)
            CreateFunction(function);
        foreach (MSSQLView view in dbDiff.ViewsToCreate)
            CreateView(view);
        foreach (MSSQLProcedure procedure in dbDiff.ProceduresToCreate)
            CreateProcedure(procedure);
        _triggerEditor.CreateTriggers(dbDiff);

        _scriptExecutor.ExecuteScripts(dbDiff, ScriptKind.AfterPublishOnce);

        if (dbDiff.NewDatabase.Version != dbDiff.OldDatabase.Version)
            QueryExecutor.Execute(new MSSQLUpdateDNDBTDbAttributesRecordQuery(dbDiff.NewDatabase));
    }

    private void RenameUserDefinedTypeToTempInDbAndInDbDiff(MSSQLUserDefinedType udt)
    {
        QueryExecutor.Execute(new MSSQLRenameUserDefinedDataTypeQuery(udt));
        udt.Name = $"_DNDBTTemp_{udt.Name}";
        QueryExecutor.Execute(new MSSQLDeleteDNDBTDbObjectRecordQuery(udt.ID));
    }

    private void CreateUserDefinedType(MSSQLUserDefinedType udt)
    {
        QueryExecutor.Execute(new MSSQLCreateTypeQuery(udt));
        QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(udt.ID, null, DbObjectType.UserDefinedType, udt.Name));
    }

    private void DropUserDefinedType(MSSQLUserDefinedType udt)
    {
        QueryExecutor.Execute(new MSSQLDropTypeQuery(udt));
    }

    private void UseNewUDTInAllTables(MSSQLUserDefinedTypeDiff udtDiff)
    {
        QueryExecutor.Execute(new MSSQLUseNewUDTInAllTablesQuery(udtDiff));
    }

    private void CreateUserDefinedTableType(MSSQLUserDefinedTableType udtt)
    {
        //QueryExecutor.Execute(new CreateTableQuery(udtt));
        QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(udtt.ID, null, DbObjectType.UserDefinedTableType, udtt.Name));
    }

    private void DropUserDefinedTableType(MSSQLUserDefinedTableType udtt)
    {
        //QueryExecutor.Execute(new DropTableQuery(udtt));
        QueryExecutor.Execute(new MSSQLDeleteDNDBTDbObjectRecordQuery(udtt.ID));
    }

    private void CreateFunction(MSSQLFunction func)
    {
        QueryExecutor.Execute(new GenericQuery($"{func.GetCode()}"));
        QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(func.ID, null, DbObjectType.Function, func.Name, func.GetCode()));
    }

    private void DropFunction(MSSQLFunction func)
    {
        QueryExecutor.Execute(new GenericQuery($"DROP FUNCTION [{func.Name}];"));
        QueryExecutor.Execute(new MSSQLDeleteDNDBTDbObjectRecordQuery(func.ID));
    }

    private void CreateView(MSSQLView view)
    {
        QueryExecutor.Execute(new CreateViewQuery(view));
        QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(view.ID, null, DbObjectType.View, view.Name, view.GetCode()));
    }

    private void DropView(MSSQLView view)
    {
        QueryExecutor.Execute(new DropViewQuery(view));
        QueryExecutor.Execute(new MSSQLDeleteDNDBTDbObjectRecordQuery(view.ID));
    }

    private void CreateProcedure(MSSQLProcedure proc)
    {
        QueryExecutor.Execute(new GenericQuery($"{proc.GetCode()}"));
        QueryExecutor.Execute(new MSSQLInsertDNDBTDbObjectRecordQuery(proc.ID, null, DbObjectType.Procedure, proc.Name, proc.GetCode()));
    }

    private void DropProcedure(MSSQLProcedure proc)
    {
        QueryExecutor.Execute(new GenericQuery($"DROP PROCEDURE [{proc.Name}];"));
        QueryExecutor.Execute(new MSSQLDeleteDNDBTDbObjectRecordQuery(proc.ID));
    }
}
