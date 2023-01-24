using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Deploy.Core.InstanceCreator;

namespace DotNetDBTools.Deploy.Core.Editors;

internal abstract class DbEditor<
    TCheckDNDBTSysTablesExistQuery,
    TCreateDNDBTSysTablesQuery,
    TDropDNDBTSysTablesQuery,
    TInsertDNDBTDbObjectRecordQuery,
    TInsertDNDBTScriptExecutionRecordQuery,
    TInsertDNDBTDbAttributesRecordQuery>
    : IDbEditor
    where TCheckDNDBTSysTablesExistQuery : NoParametersQuery, new()
    where TCreateDNDBTSysTablesQuery : NoParametersQuery, new()
    where TDropDNDBTSysTablesQuery : NoParametersQuery, new()
    where TInsertDNDBTDbObjectRecordQuery : InsertDNDBTDbObjectRecordQuery
    where TInsertDNDBTScriptExecutionRecordQuery : InsertDNDBTScriptExecutionRecordQuery
    where TInsertDNDBTDbAttributesRecordQuery : InsertDNDBTDbAttributesRecordQuery
{
    protected readonly IQueryExecutor QueryExecutor;

    protected DbEditor(IQueryExecutor queryExecutor)
    {
        QueryExecutor = queryExecutor;
    }

    public bool DNDBTSysTablesExist()
    {
        return QueryExecutor.QuerySingleOrDefault<bool>(new TCheckDNDBTSysTablesExistQuery());
    }

    public void CreateDNDBTSysTables()
    {
        QueryExecutor.Execute(new TCreateDNDBTSysTablesQuery());
    }

    public void DropDNDBTSysTables()
    {
        QueryExecutor.Execute(new TDropDNDBTSysTablesQuery());
    }

    public void PopulateDNDBTSysTables(Database database)
    {
        foreach (Table table in database.Tables)
        {
            QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(table, DbObjectType.Table));
            foreach (Column c in table.Columns)
                QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(c, DbObjectType.Column, c.GetDefault()));
            PrimaryKey pk = table.PrimaryKey;
            if (pk is not null)
                QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(pk, DbObjectType.PrimaryKey));
            foreach (UniqueConstraint uc in table.UniqueConstraints)
                QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(uc, DbObjectType.UniqueConstraint));
            foreach (CheckConstraint ck in table.CheckConstraints)
                QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(ck, DbObjectType.CheckConstraint, ck.GetExpression()));
            foreach (Index idx in table.Indexes)
                QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(idx, DbObjectType.Index));
            foreach (Trigger trg in table.Triggers)
                QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(trg, DbObjectType.Trigger, trg.GetCreateStatement()));
            foreach (ForeignKey fk in table.ForeignKeys)
                QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(fk, DbObjectType.ForeignKey));
        }
        foreach (View view in database.Views)
            QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(view, DbObjectType.View, view.GetCreateStatement()));

        PopulateDNDBTSysTablesWithAdditionalObjects(database);

        foreach (Script script in database.Scripts)
            QueryExecutor.Execute(Create<TInsertDNDBTScriptExecutionRecordQuery>(script, -1));
        QueryExecutor.Execute(Create<TInsertDNDBTDbAttributesRecordQuery>(database));
    }
    protected virtual void PopulateDNDBTSysTablesWithAdditionalObjects(Database database) { }

    public abstract void ApplyDatabaseDiff(DatabaseDiff databaseDiff, DeployOptions options);
}
