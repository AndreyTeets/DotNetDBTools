using DotNetDBTools.Generation;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Queries.DDL;

internal class DropViewQuery : NoParametersQuery
{
    public override string Sql => _sql;
    private readonly string _sql;

    public DropViewQuery(View view)
    {
        _sql = GenerationManager.GenerateSqlDropStatement(view);
    }
}
