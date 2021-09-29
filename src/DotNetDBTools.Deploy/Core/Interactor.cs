using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core
{
    public abstract class Interactor
    {
        protected readonly IQueryExecutor QueryExecutor;

        protected Interactor(IQueryExecutor queryExecutor)
        {
            QueryExecutor = queryExecutor;
        }

        public abstract bool DatabaseExists(string databaseName);
        public abstract void CreateDatabase(string databaseName);
        public abstract DatabaseInfo GetDatabaseModelFromDNDBTSysInfo();
        public abstract DatabaseInfo GenerateDatabaseModelFromDBMSSysInfo();
        public abstract void ApplyDatabaseDiff(DatabaseDiff databaseDiff);
        public abstract bool DNDBTSysTablesExist();
        public abstract void CreateDNDBTSysTables();
        public abstract void DropDNDBTSysTables();
        public abstract void PopulateDNDBTSysTables(DatabaseInfo database);
    }
}
