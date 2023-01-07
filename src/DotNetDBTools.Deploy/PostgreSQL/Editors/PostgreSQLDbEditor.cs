using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Generation.PostgreSQL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Deploy.PostgreSQL.Editors;

internal class PostgreSQLDbEditor : DbEditor<
    PostgreSQLCheckDNDBTSysTablesExistQuery,
    PostgreSQLCreateDNDBTSysTablesQuery,
    PostgreSQLDropDNDBTSysTablesQuery>
{
    private readonly IScriptExecutor _scriptExecutor;
    private readonly ITableEditor _tableEditor;
    private readonly PostgreSQLIsDependencyOfTablesObjectsEditor _isDependencyOfTablesObjectsEditor;
    private readonly PostgreSQLDependsOnTablesObjectsEditor _dependsOnTablesObjectsEditor;

    public PostgreSQLDbEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor)
    {
        _scriptExecutor = new PostgreSQLScriptExecutor(queryExecutor);
        _tableEditor = new PostgreSQLTableEditor(queryExecutor);
        _isDependencyOfTablesObjectsEditor = new PostgreSQLIsDependencyOfTablesObjectsEditor(queryExecutor);
        _dependsOnTablesObjectsEditor = new PostgreSQLDependsOnTablesObjectsEditor(queryExecutor);
    }

    public override void PopulateDNDBTSysTables(Database database)
    {
        PostgreSQLDatabase db = (PostgreSQLDatabase)database;
        InsertUserDefinedTypesInfos(db);
        InsertTablesInfos(db);
        InsertViewsFunctionsProceduresInfos(db);
        foreach (Script script in db.Scripts)
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTScriptExecutionRecordQuery(script, -1));
        QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbAttributesRecordQuery(database));
    }

    public override void ApplyDatabaseDiff(DatabaseDiff databaseDiff, DeployOptions options)
    {
        PostgreSQLDatabaseDiff dbDiff = (PostgreSQLDatabaseDiff)databaseDiff;
        if (options.NoTransaction)
            ApplyDatabaseDiff(dbDiff);
        else
            QueryExecutor.ExecuteInTransaction(() => ApplyDatabaseDiff(dbDiff));
    }

    private void ApplyDatabaseDiff(PostgreSQLDatabaseDiff dbDiff)
    {
        _scriptExecutor.DeleteRemovedScriptsExecutionRecords(dbDiff);
        _scriptExecutor.ExecuteScripts(dbDiff, ScriptKind.BeforePublishOnce);

        _dependsOnTablesObjectsEditor.DropObjectsThatDependOnTables(dbDiff);

        _isDependencyOfTablesObjectsEditor.Rename_RemovedOrChanged_ObjectsThatTablesDependOn_ToTemp_InDbAndInDbDiff(dbDiff);
        _isDependencyOfTablesObjectsEditor.Create_AddedOrChanged_ObjectsThatTablesDependOn(dbDiff);

        _tableEditor.DropTables(dbDiff);
        _tableEditor.AlterTables(dbDiff);
        _tableEditor.CreateTables(dbDiff);

        _isDependencyOfTablesObjectsEditor.Drop_RemovedOrChanged_ObjectsThatTablesDependOn(dbDiff);

        _dependsOnTablesObjectsEditor.CreateObjectsThatDependOnTables(dbDiff);

        _scriptExecutor.ExecuteScripts(dbDiff, ScriptKind.AfterPublishOnce);

        if (dbDiff.NewDatabase.Version != dbDiff.OldDatabase.Version)
            QueryExecutor.Execute(new PostgreSQLUpdateDNDBTDbAttributesRecordQuery(dbDiff.NewDatabase));
    }

    private void InsertUserDefinedTypesInfos(PostgreSQLDatabase db)
    {
        foreach (PostgreSQLCompositeType type in db.CompositeTypes)
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(type.ID, null, DbObjectType.UserDefinedType, type.Name));
        foreach (PostgreSQLDomainType type in db.DomainTypes)
        {
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(type.ID, null, DbObjectType.UserDefinedType, type.Name, type.GetDefault()));
            foreach (CheckConstraint ck in type.CheckConstraints)
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(ck.ID, type.ID, DbObjectType.CheckConstraint, ck.Name, ck.GetExpression()));
        }
        foreach (PostgreSQLEnumType type in db.EnumTypes)
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(type.ID, null, DbObjectType.UserDefinedType, type.Name));
        foreach (PostgreSQLRangeType type in db.RangeTypes)
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(type.ID, null, DbObjectType.UserDefinedType, type.Name));
    }

    private void InsertTablesInfos(PostgreSQLDatabase db)
    {
        foreach (PostgreSQLTable table in db.Tables)
        {
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(table.ID, null, DbObjectType.Table, table.Name));
            foreach (Column c in table.Columns)
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(c.ID, table.ID, DbObjectType.Column, c.Name, c.GetDefault()));

            PrimaryKey pk = table.PrimaryKey;
            if (pk is not null)
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(pk.ID, table.ID, DbObjectType.PrimaryKey, pk.Name));

            foreach (UniqueConstraint uc in table.UniqueConstraints)
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(uc.ID, table.ID, DbObjectType.UniqueConstraint, uc.Name));
            foreach (ForeignKey fk in table.ForeignKeys)
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(fk.ID, table.ID, DbObjectType.ForeignKey, fk.Name));
            foreach (CheckConstraint ck in table.CheckConstraints)
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(ck.ID, table.ID, DbObjectType.CheckConstraint, ck.Name, ck.GetExpression()));
            foreach (Index idx in table.Indexes)
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(idx.ID, table.ID, DbObjectType.Index, idx.Name));
            foreach (Trigger trg in table.Triggers)
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(trg.ID, table.ID, DbObjectType.Trigger, trg.Name, trg.GetCreateStatement()));
        }
    }

    private void InsertViewsFunctionsProceduresInfos(PostgreSQLDatabase db)
    {
        foreach (PostgreSQLView view in db.Views)
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(view.ID, null, DbObjectType.View, view.Name, view.GetCreateStatement()));
        foreach (PostgreSQLFunction func in db.Functions)
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(func.ID, null, DbObjectType.Function, func.Name, func.GetCreateStatement()));
        foreach (PostgreSQLProcedure proc in db.Procedures)
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(proc.ID, null, DbObjectType.Procedure, proc.Name, proc.GetCreateStatement()));
    }
}
