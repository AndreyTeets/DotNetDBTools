using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Generation;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;

internal class PostgreSQLCreateFunctionQuery : NoParametersQuery
{
    public override string Sql => _sql;
    private readonly string _sql;

    public PostgreSQLCreateFunctionQuery(PostgreSQLFunction func)
    {
        _sql = GenerationManager.GenerateSqlCreateStatement(func, false);
    }
}
