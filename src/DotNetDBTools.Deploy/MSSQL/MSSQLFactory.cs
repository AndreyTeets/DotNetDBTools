using System.Data.Common;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.MSSQL
{
    internal class MSSQLFactory : IFactory
    {
        public IQueryExecutor CreateQueryExecutor(DbConnection connection)
        {
            return new MSSQLQueryExecutor(connection);
        }

        public IGenSqlScriptQueryExecutor CreateGenSqlScriptQueryExecutor()
        {
            return new MSSQLGenSqlScriptQueryExecutor();
        }

        public Interactor CreateInteractor(IQueryExecutor queryExecutor)
        {
            return new MSSQLInteractor(queryExecutor);
        }
    }
}
