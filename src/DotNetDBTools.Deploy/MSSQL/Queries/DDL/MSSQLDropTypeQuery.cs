using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DDL;

internal class MSSQLDropTypeQuery : IQuery
{
    public string Sql => _sql;
    public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

    private readonly string _sql;

    public MSSQLDropTypeQuery(MSSQLUserDefinedType userDefinedType)
    {
        _sql = GetSql(userDefinedType);
    }

    private static string GetSql(MSSQLUserDefinedType userDefinedType)
    {
        string query =
$@"DROP TYPE {userDefinedType.Name};";

        return query;
    }
}
