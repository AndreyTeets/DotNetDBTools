using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Factories;

namespace DotNetDBTools.Deploy.MSSQL.Factories
{
    internal class MSSQLInteractorFactory : IInteractorFactory
    {
        public Interactor Create(IQueryExecutor queryExecutor)
        {
            return new MSSQLInteractor(queryExecutor);
        }
    }
}
