using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Generation;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;

internal class PostgreSQLDropFunctionQuery : NoParametersQuery
{
    public override string Sql => _sql;
    private readonly string _sql;

    public PostgreSQLDropFunctionQuery(PostgreSQLFunction func)
    {
        _sql = GenerationManager.GenerateSqlDropStatement(func);
    }
}
