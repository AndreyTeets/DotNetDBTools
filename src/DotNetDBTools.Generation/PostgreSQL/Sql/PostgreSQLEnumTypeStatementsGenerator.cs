using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Generation.PostgreSQL.Sql;

internal class PostgreSQLEnumTypeStatementsGenerator : StatementsGenerator<PostgreSQLEnumType>
{
    protected override string GetCreateSqlImpl(PostgreSQLEnumType type)
    {
        string res =
$@"{GetIdDeclarationText(type, 0)}CREATE TYPE ""{type.Name}"" AS ENUM
(
{GetAllowedValuesDefinitionsText(type)}
);";

        return res;
    }

    protected override string GetDropSqlImpl(PostgreSQLEnumType type)
    {
        return $@"DROP TYPE ""{type.Name}"";";
    }

    private string GetAllowedValuesDefinitionsText(PostgreSQLEnumType type)
    {
        List<string> allowedValuesDefinitions = new();

        allowedValuesDefinitions.AddRange(type.AllowedValues.Select(av =>
$@"    '{av}'"));

        return string.Join(",\n", allowedValuesDefinitions);
    }
}
