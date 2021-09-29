using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Factories;

namespace DotNetDBTools.Deploy.SQLite.Factories
{
    internal class SQLiteQueryExecutorFactory : IQueryExecutorFactory
    {
        public IQueryExecutor Create(string connectionString)
        {
            return new SQLiteQueryExecutor(connectionString);
        }
    }
}
