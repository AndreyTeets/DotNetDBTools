using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Editors;

internal abstract class DbEditor<
    TCheckDNDBTSysTablesExistQuery,
    TCreateDNDBTSysTablesQuery,
    TDropDNDBTSysTablesQuery>
    : IDbEditor
    where TCheckDNDBTSysTablesExistQuery : NoParametersQuery, new()
    where TCreateDNDBTSysTablesQuery : NoParametersQuery, new()
    where TDropDNDBTSysTablesQuery : NoParametersQuery, new()
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

    public abstract void PopulateDNDBTSysTables(Database database);
    public abstract void ApplyDatabaseDiff(DatabaseDiff databaseDiff, DeployOptions options);
}
