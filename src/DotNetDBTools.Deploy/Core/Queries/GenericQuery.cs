namespace DotNetDBTools.Deploy.Core.Queries;

internal class GenericQuery : NoParametersQuery
{
    public override string Sql => _sql;
    private readonly string _sql;

    public GenericQuery(string sql)
    {
        _sql = sql;
    }
}
