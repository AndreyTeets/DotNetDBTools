using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Queries.DDL;

internal abstract class AlterTableQuery : IQuery
{
    public string Sql => _sql;
    public IEnumerable<QueryParameter> Parameters => _parameters;

    private readonly string _sql;
    private readonly List<QueryParameter> _parameters;

    public AlterTableQuery(TableDiff tableDiff)
    {
        _sql = GetSql(tableDiff);
        _parameters = new List<QueryParameter>();
    }

    protected abstract string GetSql(TableDiff tableDiff);
}
