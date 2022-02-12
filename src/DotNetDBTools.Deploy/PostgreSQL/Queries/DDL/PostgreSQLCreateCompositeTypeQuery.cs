using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;

internal class PostgreSQLCreateCompositeTypeQuery : IQuery
{
    public string Sql => _sql;
    public IEnumerable<QueryParameter> Parameters => _parameters;

    private readonly string _sql;
    private readonly List<QueryParameter> _parameters;

    public PostgreSQLCreateCompositeTypeQuery(PostgreSQLCompositeType type)
    {
        _sql = GetSql(type);
        _parameters = new List<QueryParameter>();
    }

    private static string GetSql(PostgreSQLCompositeType type)
    {
        string query =
$@"CREATE TYPE ""{type.Name}"" AS
(
{GetAttributesDefinitionsText(type)}
);";

        return query;
    }

    private static string GetAttributesDefinitionsText(PostgreSQLCompositeType type)
    {
        List<string> attributesDefinitions = new();

        attributesDefinitions.AddRange(type.Attributes.Select(a =>
$@"    ""{a.Name}"" {a.DataType.GetQuotedName()}"));

        return string.Join(",\n", attributesDefinitions);
    }
}
