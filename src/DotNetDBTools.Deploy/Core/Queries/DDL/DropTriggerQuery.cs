using DotNetDBTools.Generation;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Queries.DDL;

internal class DropTriggerQuery : NoParametersQuery
{
    public override string Sql => _sql;
    private readonly string _sql;

    public DropTriggerQuery(Trigger trigger)
    {
        _sql = GenerationManager.GenerateSqlDropStatement(trigger);
    }
}
