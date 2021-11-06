namespace DotNetDBTools.Deploy.Core
{
    internal interface IFactory
    {
        public IQueryExecutor CreateQueryExecutor(string connectionString);
        public IGenSqlScriptQueryExecutor CreateGenSqlScriptQueryExecutor();
        public Interactor CreateInteractor(IQueryExecutor queryExecutor);
    }
}
