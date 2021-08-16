using System;
using System.Threading.Tasks;

namespace DotNetDBTools.DeployInteractor.SQLite
{
    public class SQLiteQueryExecutor : IQueryExecutor
    {
        public Task<object> Execute(string query, params QueryParameter[] parameters)
        {
            Console.WriteLine($"Query:\n{query}\n");
            return null;
        }
    }
}
