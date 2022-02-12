using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Common.Queries.DDL;

internal abstract class DropIndexQuery : IQuery
{
    public string Sql => _sql;
    public IEnumerable<QueryParameter> Parameters => _parameters;

    private readonly string _sql;
    private readonly List<QueryParameter> _parameters;

    public DropIndexQuery(Index index, Table table)
    {
        _sql = GetSql(index, table);
        _parameters = new List<QueryParameter>();
    }

    protected abstract string GetSql(Index index, Table table);
}
