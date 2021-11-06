using System.Data.Common;

namespace DotNetDBTools.Deploy.Core
{
    internal interface IFactory
    {
        public IQueryExecutor CreateQueryExecutor(DbConnection connection);
        public IGenSqlScriptQueryExecutor CreateGenSqlScriptQueryExecutor();
        public Interactor CreateInteractor(IQueryExecutor queryExecutor);
    }
}
