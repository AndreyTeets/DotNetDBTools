using System;
using System.Threading.Tasks;

namespace DotNetDBTools.DeployInteractor.MSSQL
{
    public class MSSQLQueryExecutor : IQueryExecutor
    {
        public Task<object> Execute(string query, params QueryParameter[] parameters)
        {
            Console.WriteLine($"Query:\n{query}\n");
            return null;
        }
    }
}
