using System;
using System.Collections.Generic;

namespace DotNetDBTools.DeployInteractor.MSSQL
{
    public class MSSQLQueryExecutor : IQueryExecutor
    {
        public int Execute(IQuery query)
        {
            Console.WriteLine($"Query:\n{query.Sql}\n");
            return 0;
        }

        public IEnumerable<TOut> Query<TOut>(IQuery query)
        {
            throw new NotImplementedException();
        }

        public TOut QuerySingleOrDefault<TOut>(IQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
