using System.Collections.Generic;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Generation.PostgreSQL.Sql;

internal class PostgreSQLDomainTypeStatementsGenerator : StatementsGenerator<PostgreSQLDomainType>
{
    protected override string GetCreateSqlImpl(PostgreSQLDomainType type)
    {
        string res =
$@"{GetIdDeclarationText(type, 0)}CREATE DOMAIN ""{type.Name}"" AS
{GetDefinitionsText(type)};";

        return res;
    }

    protected override string GetDropSqlImpl(PostgreSQLDomainType type)
    {
        return $@"DROP DOMAIN ""{type.Name}"";";
    }

    private string GetDefinitionsText(PostgreSQLDomainType type)
    {
        List<string> definitions = new();

        definitions.Add(
$@"    {type.UnderlyingType.Name} {Statements.Nullability(type)}{Statements.Default(type)}");

        foreach (CheckConstraint ck in type.CheckConstraints)
        {
            definitions.Add(
$@"    {GetIdDeclarationText(ck, 4)}{Statements.DefCheckConstraint(ck)}");
        }

        return string.Join("\n", definitions);
    }

    private static class Statements
    {
        public static string Nullability(PostgreSQLDomainType type) =>
type.NotNull ? "NOT NULL" : "NULL"
            ;
        public static string Default(PostgreSQLDomainType type) =>
type.Default is not null ? $@" DEFAULT {type.GetDefault()}" : ""
            ;

        public static string DefCheckConstraint(CheckConstraint ck) =>
$@"CONSTRAINT ""{ck.Name}"" CHECK ({ck.GetExpression()})"
            ;
    }
}
