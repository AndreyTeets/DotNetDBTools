using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.MSSQL
{
    internal class MSSQLFactory : IFactory
    {
        public IQueryExecutor CreateQueryExecutor(string connectionString)
        {
            return new MSSQLQueryExecutor(connectionString);
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
