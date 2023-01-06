using DotNetDBTools.Generation;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Queries.DDL;

internal class CreateIndexQuery : NoParametersQuery
{
    public override string Sql => _sql;
    private readonly string _sql;

    public CreateIndexQuery(Index index)
    {
        _sql = GenerationManager.GenerateSqlCreateStatement(index, false);
    }
}
