namespace DotNetDBTools.Deploy.Core;

internal interface IGenSqlScriptQueryExecutor : IQueryExecutor
{
    public bool NoDNDBTInfo { get; set; }
    public string GetFinalScript();
}
