using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.SQLite
{
    internal class SQLiteFactory : IFactory
    {
        public IQueryExecutor CreateQueryExecutor(string connectionString)
        {
            return new SQLiteQueryExecutor(connectionString);
        }

        public IGenSqlScriptQueryExecutor CreateGenSqlScriptQueryExecutor()
        {
            return new SQLiteGenSqlScriptQueryExecutor();
        }

        public Interactor CreateInteractor(IQueryExecutor queryExecutor)
        {
            return new SQLiteInteractor(queryExecutor);
        }
    }
}
