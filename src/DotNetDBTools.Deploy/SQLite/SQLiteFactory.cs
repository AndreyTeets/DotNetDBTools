using System.Data.Common;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.SQLite
{
    internal class SQLiteFactory : IFactory
    {
        public IQueryExecutor CreateQueryExecutor(DbConnection connection)
        {
            return new SQLiteQueryExecutor(connection);
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
