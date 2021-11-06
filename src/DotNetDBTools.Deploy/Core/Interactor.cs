using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core
{
    internal abstract class Interactor
    {
        protected readonly IQueryExecutor QueryExecutor;

        protected Interactor(IQueryExecutor queryExecutor)
        {
            QueryExecutor = queryExecutor;
        }

        public abstract bool DatabaseExists(string databaseName);
        public abstract void CreateDatabase(string databaseName);
        public abstract Database GetDatabaseModelFromDNDBTSysInfo();
        public abstract Database GenerateDatabaseModelFromDBMSSysInfo();
        public abstract void ApplyDatabaseDiff(DatabaseDiff databaseDiff);
        public abstract bool DNDBTSysTablesExist();
        public abstract void CreateDNDBTSysTables();
        public abstract void DropDNDBTSysTables();
        public abstract void PopulateDNDBTSysTables(Database database);
    }
}
