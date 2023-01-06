using DotNetDBTools.Generation;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Queries.DDL;

internal class CreateTriggerQuery : NoParametersQuery
{
    public override string Sql => _sql;
    private readonly string _sql;

    public CreateTriggerQuery(Trigger trigger)
    {
        _sql = GenerationManager.GenerateSqlCreateStatement(trigger, false);
    }
}
