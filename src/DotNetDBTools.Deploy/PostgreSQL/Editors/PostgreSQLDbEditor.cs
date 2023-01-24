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
    PostgreSQLDropDNDBTSysTablesQuery,
    PostgreSQLInsertDNDBTDbObjectRecordQuery,
    PostgreSQLInsertDNDBTScriptExecutionRecordQuery,
    PostgreSQLInsertDNDBTDbAttributesRecordQuery>
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

    protected override void PopulateDNDBTSysTablesWithAdditionalObjects(Database database)
    {
        PostgreSQLDatabase db = (PostgreSQLDatabase)database;
        foreach (PostgreSQLSequence sequence in db.Sequences)
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(sequence, DbObjectType.Sequence));

        foreach (PostgreSQLCompositeType type in db.CompositeTypes)
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(type, DbObjectType.UserDefinedType));
        foreach (PostgreSQLDomainType type in db.DomainTypes)
        {
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(type, DbObjectType.UserDefinedType, type.GetDefault()));
            foreach (CheckConstraint ck in type.CheckConstraints)
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(ck, DbObjectType.CheckConstraint, ck.GetExpression()));
        }
        foreach (PostgreSQLEnumType type in db.EnumTypes)
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(type, DbObjectType.UserDefinedType));
        foreach (PostgreSQLRangeType type in db.RangeTypes)
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(type, DbObjectType.UserDefinedType));

        foreach (PostgreSQLFunction func in db.Functions)
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(func, DbObjectType.Function, func.GetCreateStatement()));
        foreach (PostgreSQLProcedure proc in db.Procedures)
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(proc, DbObjectType.Procedure, proc.GetCreateStatement()));
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

        if (dbDiff.NewVersion != dbDiff.OldVersion)
            QueryExecutor.Execute(new PostgreSQLUpdateDNDBTDbAttributesRecordQuery(dbDiff.NewVersion));
    }
}
