using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;
using static DotNetDBTools.Deploy.MSSQL.MSSQLQueriesHelper;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DDL
{
    internal class MSSQLCreateTableQuery : CreateTableQuery
    {
        public MSSQLCreateTableQuery(Table table)
            : base(table) { }

        protected override string GetSql(Table table)
        {
            string query =
$@"CREATE TABLE {table.Name}
(
{GetTableDefinitionsText(table)}
);";

            foreach (Index index in table.Indexes)
            {
                string _ =
$@"CREATE INDEX {index.Name}
ON {table.Name} ({string.Join(", ", index.Columns)});";
            }

            foreach (Trigger trigger in table.Triggers)
            {
                string _ =
$@"{trigger.CodePiece}";
            }

            return query;
        }

        private static string GetTableDefinitionsText(Table table)
        {
            List<string> tableDefinitions = new();

            tableDefinitions.AddRange(table.Columns.Select(c =>
$@"    {c.Name} {c.DataType.Name}{GetIdentityStatement(c)} {GetNullabilityStatement(c)}{GetDefaultValStatement(c)}"));

            if (table.PrimaryKey is not null)
            {
                tableDefinitions.Add(
$@"    CONSTRAINT {table.PrimaryKey.Name} PRIMARY KEY ({string.Join(", ", table.PrimaryKey.Columns)})");
            }

            tableDefinitions.AddRange(table.UniqueConstraints.Select(uc =>
$@"    CONSTRAINT {uc.Name} UNIQUE ({string.Join(", ", uc.Columns)})"));

            IEnumerable<string> _ = table.CheckConstraints.Select(ck =>
$@"    CONSTRAINT {ck.Name} {ck.GetCode()}");

            return string.Join(",\n", tableDefinitions);
        }

        private static string GetDefaultValStatement(Column column)
        {
            if (column.Default is not null)
            {
                return $" CONSTRAINT {((MSSQLColumn)column).DefaultConstraintName} DEFAULT {QuoteDefaultValue(column.Default)}";
            }
            return "";
        }
    }
}
