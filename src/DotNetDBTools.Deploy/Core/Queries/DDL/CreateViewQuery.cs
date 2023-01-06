using DotNetDBTools.Generation;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Queries.DDL;

internal class CreateViewQuery : NoParametersQuery
{
    public override string Sql => _sql;
    private readonly string _sql;

    public CreateViewQuery(View view)
    {
        _sql = GenerationManager.GenerateSqlCreateStatement(view, false);
    }
}
