namespace DotNetDBTools.Deploy.Core.Factories
{
    public interface IInteractorFactory
    {
        public Interactor Create(IQueryExecutor queryExecutor);
    }
}
