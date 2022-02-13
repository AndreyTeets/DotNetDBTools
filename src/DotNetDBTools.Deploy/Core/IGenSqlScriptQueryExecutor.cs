namespace DotNetDBTools.Deploy.Core;

internal interface IGenSqlScriptQueryExecutor : IQueryExecutor
{
    public bool DDLOnly { get; set; }
    public string GetFinalScript();
}
