using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Generation.PostgreSQL.Sql;

internal class PostgreSQLCompositeTypeStatementsGenerator : StatementsGenerator<PostgreSQLCompositeType>
{
    protected override string GetCreateSqlImpl(PostgreSQLCompositeType type)
    {
        string res =
$@"{GetIdDeclarationText(type, 0)}CREATE TYPE ""{type.Name}"" AS
(
{GetAttributesDefinitionsText(type)}
);";

        return res;
    }

    protected override string GetDropSqlImpl(PostgreSQLCompositeType type)
    {
        return $@"DROP TYPE ""{type.Name}"";";
    }

    private string GetAttributesDefinitionsText(PostgreSQLCompositeType type)
    {
        List<string> attributesDefinitions = new();

        attributesDefinitions.AddRange(type.Attributes.Select(a =>
$@"    ""{a.Name}"" {a.DataType.Name}"));

        return string.Join(",\n", attributesDefinitions);
    }
}
