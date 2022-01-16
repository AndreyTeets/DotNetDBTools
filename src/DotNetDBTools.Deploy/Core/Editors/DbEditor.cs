using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Editors
{
    internal abstract class DbEditor<
        TCheckDNDBTSysTablesExistQuery,
        TCreateDNDBTSysTablesQuery,
        TDropDNDBTSysTablesQuery>
        : IDbEditor
        where TCheckDNDBTSysTablesExistQuery : SqlTextOnlyQuery, new()
        where TCreateDNDBTSysTablesQuery : SqlTextOnlyQuery, new()
        where TDropDNDBTSysTablesQuery : SqlTextOnlyQuery, new()
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
}
