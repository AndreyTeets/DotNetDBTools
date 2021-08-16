using System.Collections.Generic;

namespace DotNetDBTools.DeployInteractor
{
    public interface IQueryExecutor
    {
        public int Execute(string query, params QueryParameter[] parameters);
        public IEnumerable<TOut> Query<TOut>(string query, params QueryParameter[] parameters);
        public TOut QuerySingleOrDefault<TOut>(string query, params QueryParameter[] parameters);
    }
}
