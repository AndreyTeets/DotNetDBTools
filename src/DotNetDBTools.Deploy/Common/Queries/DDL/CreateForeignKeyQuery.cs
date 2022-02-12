using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Common.Queries.DDL;

internal abstract class CreateForeignKeyQuery : IQuery
{
    public string Sql => _sql;
    public IEnumerable<QueryParameter> Parameters => _parameters;

    private readonly string _sql;
    private readonly List<QueryParameter> _parameters;

    public CreateForeignKeyQuery(ForeignKey fk, string tableName)
    {
        _sql = GetSql(fk, tableName);
        _parameters = new List<QueryParameter>();
    }

    protected abstract string GetSql(ForeignKey fk, string tableName);
}
