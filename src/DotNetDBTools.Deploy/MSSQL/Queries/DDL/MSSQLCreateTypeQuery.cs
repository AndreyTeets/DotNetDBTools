using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DDL;

internal class MSSQLCreateTypeQuery : IQuery
{
    public string Sql => _sql;
    public IEnumerable<QueryParameter> Parameters => _parameters;

    private readonly string _sql;
    private readonly List<QueryParameter> _parameters;

    public MSSQLCreateTypeQuery(MSSQLUserDefinedType userDefinedType)
    {
        _sql = GetSql(userDefinedType);
        _parameters = new List<QueryParameter>();
    }

    private static string GetSql(MSSQLUserDefinedType userDefinedType)
    {
        string query =
$@"CREATE TYPE {userDefinedType.Name} FROM {userDefinedType.UnderlyingDataType.Name};";

        return query;
    }
}
