namespace DotNetDBTools.Deploy.Core;

internal interface IGenSqlScriptQueryExecutor : IQueryExecutor
{
    public string GetFinalScript();
}
