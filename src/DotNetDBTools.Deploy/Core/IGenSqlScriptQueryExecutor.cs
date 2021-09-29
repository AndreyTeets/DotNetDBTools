namespace DotNetDBTools.Deploy.Core
{
    public interface IGenSqlScriptQueryExecutor : IQueryExecutor
    {
        public string GetFinalScript();
    }
}
