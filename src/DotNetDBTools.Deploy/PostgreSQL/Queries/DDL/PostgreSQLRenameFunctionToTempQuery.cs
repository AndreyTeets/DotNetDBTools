using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;

internal class PostgreSQLRenameFunctionToTempQuery : IQuery
{
    public string Sql => _sql;
    public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

    private readonly string _sql;

    public PostgreSQLRenameFunctionToTempQuery(PostgreSQLFunction func)
    {
        _sql = GetSql(func);
    }

    private static string GetSql(PostgreSQLFunction func)
    {
        string tempPrefix = "_DNDBTTemp_";
        // TODO ALTER FUNCTION ""{func.Name}""({string.Join(",", func.ArgsTypes)}) RENAME TO
        return
$@"ALTER FUNCTION ""{func.Name}"" RENAME TO ""{tempPrefix}{func.Name}"";";
    }
}
