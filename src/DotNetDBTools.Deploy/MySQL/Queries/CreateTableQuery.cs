﻿using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;
using static DotNetDBTools.Deploy.MySQL.MySQLQueriesHelper;

namespace DotNetDBTools.Deploy.MySQL.Queries
{
    internal class CreateTableQuery : IQuery
    {
        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        public CreateTableQuery(MySQLTable table)
        {
            _sql = GetSql(table);
            _parameters = new List<QueryParameter>();
        }

        private static string GetSql(MySQLTable table)
        {
            string query =
$@"CREATE TABLE `{table.Name}`
(
{GetTableDefinitionsText(table)}
);";

            foreach (Index index in table.Indexes)
            {
                string _ =
$@"CREATE INDEX `{index.Name}`
ON `{table.Name}` ({string.Join(", ", index.Columns.Select(x => $@"`{x}`"))});";
            }

            foreach (Trigger trigger in table.Triggers)
            {
                string _ =
$@"{trigger.Code}";
            }

            return query;
        }

        private static string GetTableDefinitionsText(MySQLTable table)
        {
            List<string> tableDefinitions = new();

            tableDefinitions.AddRange(table.Columns.Select(c =>
$@"    `{c.Name}` {c.DataType.Name}{GetIdentityStatement(c)} {GetNullabilityStatement(c)}{GetDefaultValStatement(c)}"));

            if (table.PrimaryKey is not null)
            {
                tableDefinitions.Add(
$@"    CONSTRAINT `{table.PrimaryKey.Name}` PRIMARY KEY ({string.Join(", ", table.PrimaryKey.Columns.Select(x => $@"`{x}`"))})");
            }

            tableDefinitions.AddRange(table.UniqueConstraints.Select(uc =>
$@"    CONSTRAINT `{uc.Name}` UNIQUE ({string.Join(", ", uc.Columns.Select(x => $@"`{x}`"))})"));

            IEnumerable<string> _ = table.CheckConstraints.Select(cc =>
$@"    CONSTRAINT `{cc.Name}` CHECK ({cc.Code})");

            return string.Join(",\n", tableDefinitions);
        }

        private static string GetDefaultValStatement(Column column)
        {
            if (column.Default is not null)
            {
                return $" DEFAULT {QuoteDefaultValue(column.Default)}";
            }
            return "";
        }
    }
}