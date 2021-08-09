using System.Threading.Tasks;

namespace DotNetDBTools.DeployInteractor
{
    public interface IQueryExecutor
    {
        public Task<object> Execute(string query, params QueryParameter[] parameters);
    }
}
