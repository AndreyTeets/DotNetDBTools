using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Generation;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;

internal class PostgreSQLDropSequenceQuery : NoParametersQuery
{
    public override string Sql => _sql;
    private readonly string _sql;

    public PostgreSQLDropSequenceQuery(PostgreSQLSequence sequence)
    {
        _sql = GenerationManager.GenerateSqlDropStatement(sequence);
    }
}
