using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;

internal class PostgreSQLCreateRangeTypeQuery : IQuery
{
    public string Sql => _sql;
    public IEnumerable<QueryParameter> Parameters => _parameters;

    private readonly string _sql;
    private readonly List<QueryParameter> _parameters;

    public PostgreSQLCreateRangeTypeQuery(PostgreSQLRangeType type)
    {
        _sql = GetSql(type);
        _parameters = new List<QueryParameter>();
    }

    private static string GetSql(PostgreSQLRangeType type)
    {
        string query =
$@"CREATE TYPE ""{type.Name}"" AS RANGE
(
{GetRangeTypeDefinitionsText(type)}
);";

        return query;
    }

    private static string GetRangeTypeDefinitionsText(PostgreSQLRangeType type)
    {
        List<string> definitions = new();

        definitions.Add(
$@"    SUBTYPE = {type.Subtype.GetQuotedName()}");

        if (type.SubtypeOperatorClass is not null)
        {
            definitions.Add(
$@"    SUBTYPE_OPCLASS = ""{type.SubtypeOperatorClass}""");
        }

        if (type.Collation is not null)
        {
            definitions.Add(
$@"    COLLATION = ""{type.Collation}""");
        }

        if (type.CanonicalFunction is not null)
        {
            definitions.Add(
$@"    CANONICAL  = ""{type.CanonicalFunction}""");
        }

        if (type.SubtypeDiff is not null)
        {
            definitions.Add(
$@"    SUBTYPE_DIFF  = ""{type.SubtypeDiff}""");
        }

        if (type.MultirangeTypeName is not null)
        {
            definitions.Add(
$@"    MULTIRANGE_TYPE_NAME = ""{type.MultirangeTypeName}""");
        }

        return string.Join(",\n", definitions);
    }
}
