namespace DotNetDBTools.Deploy.Core.Factories
{
    public interface IQueryExecutorFactory
    {
        public IQueryExecutor Create(string connectionString);
    }
}
