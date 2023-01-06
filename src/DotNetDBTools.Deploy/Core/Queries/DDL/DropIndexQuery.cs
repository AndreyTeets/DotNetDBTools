using DotNetDBTools.Generation;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Queries.DDL;

internal class DropIndexQuery : NoParametersQuery
{
    public override string Sql => _sql;
    private readonly string _sql;

    public DropIndexQuery(Index index)
    {
        _sql = GenerationManager.GenerateSqlDropStatement(index);
    }
}
