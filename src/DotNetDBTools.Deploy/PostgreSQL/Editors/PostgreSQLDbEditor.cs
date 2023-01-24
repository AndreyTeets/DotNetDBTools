using DotNetDBTools.Deploy.Common.Editors;
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
    private readonly IScriptExecutor _scriptsExecutor;
    private readonly PostgreSQLSequencesEditor _sequencesEditor;
    private readonly PostgreSQLTablesEditor _tablesEditor;
    private readonly PostgreSQLTypesEditor _typesEditor;
    private readonly PostgreSQLProgrammableObjectsEditor _programmableObjectsEditor;
    private readonly IIndexEditor _indexesEditor;
    private readonly ITriggerEditor _triggersEditor;
    private readonly IForeignKeyEditor _foreignKeysEditor;

    public PostgreSQLDbEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor)
    {
        _scriptsExecutor = new PostgreSQLScriptExecutor(queryExecutor);
        _sequencesEditor = new PostgreSQLSequencesEditor(queryExecutor);
        _tablesEditor = new PostgreSQLTablesEditor(queryExecutor);
        _typesEditor = new PostgreSQLTypesEditor(queryExecutor);
        _programmableObjectsEditor = new PostgreSQLProgrammableObjectsEditor(queryExecutor);
        _indexesEditor = new PostgreSQLIndexesEditor(queryExecutor);
        _triggersEditor = new PostgreSQLTriggersEditor(queryExecutor);
        _foreignKeysEditor = new PostgreSQLForeignKeysEditor(queryExecutor);
    }

    public override void PopulateDNDBTSysTables(Database database)
    {
        PostgreSQLDatabase db = (PostgreSQLDatabase)database;
        foreach (PostgreSQLSequence sequence in db.Sequences)
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(sequence.ID, null, DbObjectType.Sequence, sequence.Name));
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
        _scriptsExecutor.DeleteRemovedScriptsExecutionRecords(dbDiff);
        _scriptsExecutor.ExecuteScripts(dbDiff, ScriptKind.BeforePublishOnce);

        _triggersEditor.DropTriggers(dbDiff);
        _foreignKeysEditor.DropForeignKeys(dbDiff);
        _indexesEditor.DropIndexes(dbDiff);
        _tablesEditor.DropCheckConstraints(dbDiff);
        _tablesEditor.DropColumnsDefault(dbDiff);
        _typesEditor.DropDomainsCheckConstraints(dbDiff);
        _typesEditor.DropDomainsDefault(dbDiff);

        _programmableObjectsEditor.DropComplexDepsProgrammableObjects(dbDiff);

        _sequencesEditor.DropOwnerAndRename_SequencesToDrop_ToTemp(dbDiff);
        _programmableObjectsEditor.Rename_SimpleDepsProgrammableObjectsToDrop_ToTemp(dbDiff);
        _typesEditor.Rename_TypesToDrop_ToTemp(dbDiff);

        _sequencesEditor.AlterSequencesExceptOwners(dbDiff);
        _sequencesEditor.CreateSequencesWithoutOwners(dbDiff);
        _programmableObjectsEditor.CreateSimpleDepsProgrammableObjects(dbDiff);
        _typesEditor.AlterTypes_ExceptDomains_Default_CK(dbDiff);
        _typesEditor.CreateTypes(dbDiff);

        _tablesEditor.DropTables(dbDiff);
        _tablesEditor.AlterTables_Except_ColumnsDefault_CK_FK(dbDiff);
        _tablesEditor.CreateTables_Without_ColumnsDefault_CK_FK(dbDiff);
        _sequencesEditor.SetSequencesOwners(dbDiff);

        _typesEditor.Drop_RenamedToTemp_TypesToDrop(dbDiff);
        _programmableObjectsEditor.Drop_RenamedToTemp_SimpleDepsProgrammableObjectsToDrop(dbDiff);
        _sequencesEditor.Drop_RenamedToTemp_SequencesToDrop(dbDiff);

        _programmableObjectsEditor.CreateComplexDepsProgrammableObjects(dbDiff);

        _typesEditor.SetDomainsDefault(dbDiff);
        _typesEditor.AddDomainsCheckConstraints(dbDiff);
        _tablesEditor.SetColumnsDefault(dbDiff);
        _tablesEditor.AddCheckConstraints(dbDiff);
        _indexesEditor.CreateIndexes(dbDiff);
        _foreignKeysEditor.CreateForeignKeys(dbDiff);
        _triggersEditor.CreateTriggers(dbDiff);

        _scriptsExecutor.ExecuteScripts(dbDiff, ScriptKind.AfterPublishOnce);

        if (dbDiff.NewDatabaseVersion != dbDiff.OldDatabaseVersion)
            QueryExecutor.Execute(new PostgreSQLUpdateDNDBTDbAttributesRecordQuery(dbDiff.NewDatabaseVersion));
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
