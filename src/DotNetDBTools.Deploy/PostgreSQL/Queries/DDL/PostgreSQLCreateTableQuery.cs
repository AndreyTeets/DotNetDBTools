using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Deploy.PostgreSQL.PostgreSQLQueriesHelper;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL
{
    internal class PostgreSQLCreateTableQuery : CreateTableQuery
    {
        public PostgreSQLCreateTableQuery(Table table)
            : base(table) { }

        protected override string GetSql(Table table)
        {
            string query =
$@"CREATE TABLE ""{table.Name}""
(
{GetTableDefinitionsText(table)}
);";

            return query;
        }

        private static string GetTableDefinitionsText(Table table)
        {
            List<string> tableDefinitions = new();

            tableDefinitions.AddRange(table.Columns.Select(c =>
$@"    ""{c.Name}"" {c.DataType.GetQuotedName()}{GetIdentityStatement(c)} {GetNullabilityStatement(c)}{GetDefaultValStatement(c)}"));

            if (table.PrimaryKey is not null)
            {
                tableDefinitions.Add(
$@"    CONSTRAINT ""{table.PrimaryKey.Name}"" PRIMARY KEY ({string.Join(", ", table.PrimaryKey.Columns.Select(x => $@"""{x}"""))})");
            }

            tableDefinitions.AddRange(table.UniqueConstraints.Select(uc =>
$@"    CONSTRAINT ""{uc.Name}"" UNIQUE ({string.Join(", ", uc.Columns.Select(x => $@"""{x}"""))})"));

            tableDefinitions.AddRange(table.CheckConstraints.Select(ck =>
$@"    CONSTRAINT ""{ck.Name}"" {ck.GetCode()}"));

            return string.Join(",\n", tableDefinitions);
        }

        private static string GetDefaultValStatement(Column column)
        {
            if (column.Default is not null)
            {
                return $@" DEFAULT {QuoteDefaultValue(column.Default)}";
            }
            return "";
        }
    }
}
