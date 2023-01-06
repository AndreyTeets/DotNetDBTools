using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Generation;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;

internal class PostgreSQLCreateDomainTypeQuery : NoParametersQuery
{
    public override string Sql => _sql;
    private readonly string _sql;

    public PostgreSQLCreateDomainTypeQuery(PostgreSQLDomainType type)
    {
        _sql = GenerationManager.GenerateSqlCreateStatement(type, false);
    }
}
