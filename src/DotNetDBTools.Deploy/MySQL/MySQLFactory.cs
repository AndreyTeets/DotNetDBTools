using System.Data.Common;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.MySQL
{
    internal class MySQLFactory : IFactory
    {
        public IQueryExecutor CreateQueryExecutor(DbConnection connection)
        {
            return new MySQLQueryExecutor(connection);
        }

        public IGenSqlScriptQueryExecutor CreateGenSqlScriptQueryExecutor()
        {
            return new MySQLGenSqlScriptQueryExecutor();
        }

        public Interactor CreateInteractor(IQueryExecutor queryExecutor)
        {
            return new MySQLInteractor(queryExecutor);
        }
    }
}
