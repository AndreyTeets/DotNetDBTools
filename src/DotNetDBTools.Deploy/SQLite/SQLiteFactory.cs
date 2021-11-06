using DotNetDBTools.Deploy.Core;
using Microsoft.Data.Sqlite;

namespace DotNetDBTools.Deploy.SQLite
{
    internal class SQLiteFactory : IFactory
    {
        public IQueryExecutor CreateQueryExecutor(string connectionString)
        {
            return new SQLiteQueryExecutor(() => new SqliteConnection(connectionString));
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
