using System.Collections.Generic;

namespace DotNetDBTools.Deploy.Core
{
    public interface IQueryExecutor
    {
        public void BeginTransaction();
        public void CommitTransaction();
        public void RollbackTransaction();
        public int Execute(IQuery query);
        public IEnumerable<TOut> Query<TOut>(IQuery query);
        public TOut QuerySingleOrDefault<TOut>(IQuery query);
    }
}
