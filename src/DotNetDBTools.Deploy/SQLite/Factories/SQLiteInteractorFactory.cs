using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Factories;

namespace DotNetDBTools.Deploy.SQLite.Factories
{
    internal class SQLiteInteractorFactory : IInteractorFactory
    {
        public Interactor Create(IQueryExecutor queryExecutor)
        {
            return new SQLiteInteractor(queryExecutor);
        }
    }
}
