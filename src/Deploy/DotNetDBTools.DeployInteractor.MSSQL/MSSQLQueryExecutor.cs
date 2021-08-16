using System;
using System.Collections.Generic;

namespace DotNetDBTools.DeployInteractor.MSSQL
{
    public class MSSQLQueryExecutor : IQueryExecutor
    {
        public int Execute(string query, params QueryParameter[] parameters)
        {
            Console.WriteLine($"Query:\n{query}\n");
            return 0;
        }

        public IEnumerable<TOut> Query<TOut>(string query, params QueryParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public TOut QuerySingleOrDefault<TOut>(string query, params QueryParameter[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
