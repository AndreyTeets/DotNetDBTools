using System.Collections.Generic;
using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Generation.PostgreSQL.Sql;

internal class PostgreSQLRangeTypeStatementsGenerator : StatementsGenerator<PostgreSQLRangeType>
{
    protected override string GetCreateSqlImpl(PostgreSQLRangeType type)
    {
        string res =
$@"{GetIdDeclarationText(type, 0)}CREATE TYPE ""{type.Name}"" AS RANGE
(
{GetDefinitionsText(type)}
);";

        return res;
    }

    protected override string GetDropSqlImpl(PostgreSQLRangeType type)
    {
        return $@"DROP TYPE ""{type.Name}"";";
    }

    private string GetDefinitionsText(PostgreSQLRangeType type)
    {
        List<string> definitions = new();

        definitions.Add(
$@"    SUBTYPE = {type.Subtype.Name}");

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
