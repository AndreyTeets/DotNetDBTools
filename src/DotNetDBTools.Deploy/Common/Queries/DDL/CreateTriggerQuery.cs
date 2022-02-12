using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Common.Queries.DDL;

internal abstract class CreateTriggerQuery : IQuery
{
    public string Sql => _sql;
    public IEnumerable<QueryParameter> Parameters => _parameters;

    private readonly string _sql;
    private readonly List<QueryParameter> _parameters;

    public CreateTriggerQuery(Trigger trigger)
    {
        _sql = GetSql(trigger);
        _parameters = new List<QueryParameter>();
    }

    protected abstract string GetSql(Trigger trigger);
}
