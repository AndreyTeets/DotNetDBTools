using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Generation;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;

internal class PostgreSQLDropTypeQuery : NoParametersQuery
{
    public override string Sql => _sql;
    private readonly string _sql;

    public PostgreSQLDropTypeQuery(DbObject type)
    {
        _sql = GenerationManager.GenerateSqlDropStatement(type);
    }
}
