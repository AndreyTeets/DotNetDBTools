using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Generation;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;

internal class PostgreSQLDropProcedureQuery : NoParametersQuery
{
    public override string Sql => _sql;
    private readonly string _sql;

    public PostgreSQLDropProcedureQuery(PostgreSQLProcedure proc)
    {
        _sql = GenerationManager.GenerateSqlDropStatement(proc);
    }
}
