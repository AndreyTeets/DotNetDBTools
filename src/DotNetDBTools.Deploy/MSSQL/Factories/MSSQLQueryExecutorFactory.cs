using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Factories;

namespace DotNetDBTools.Deploy.MSSQL.Factories
{
    internal class MSSQLQueryExecutorFactory : IQueryExecutorFactory
    {
        public IQueryExecutor Create(string connectionString)
        {
            return new MSSQLQueryExecutor(connectionString);
        }
    }
}
