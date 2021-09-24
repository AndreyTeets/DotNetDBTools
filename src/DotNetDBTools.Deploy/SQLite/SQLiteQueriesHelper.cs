using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;
using static DotNetDBTools.Deploy.SQLite.SQLiteSqlTypeMapper;

namespace DotNetDBTools.Deploy.SQLite
{
    public static class SQLiteQueriesHelper
    {
        public static string GetTableDefinitionsText(SQLiteTableInfo table)
        {
            List<string> tableDefinitions = new();

            tableDefinitions.AddRange(table.Columns.Select(column =>
$@"    {column.Name} {GetSqlType(column.DataType)} {GetNullabilityStatement(column)} {GetDefaultValStatement(table.Name, column)}"));

            if (table.PrimaryKey is not null)
            {
                tableDefinitions.Add(
$@"    CONSTRAINT {table.PrimaryKey.Name} PRIMARY KEY ({string.Join(", ", table.PrimaryKey.Columns)})");
            }

            tableDefinitions.AddRange(table.UniqueConstraints.Select(uc =>
$@"    CONSTRAINT {uc.Name} UNIQUE ({string.Join(", ", uc.Columns)})"));

            tableDefinitions.AddRange(table.ForeignKeys.Select(fk =>
$@"    CONSTRAINT {fk.Name} FOREIGN KEY ({string.Join(",", fk.ThisColumnNames)}) REFERENCES {fk.ForeignTableName}({string.Join(",", fk.ForeignColumnNames)})"));

            return string.Join(",\n", tableDefinitions);
        }

        private static string GetDefaultValStatement(string tableName, ColumnInfo column)
        {
            if (column.Default is not null)
            {
                return $"CONSTRAINT DF_{tableName}_{column.Name} DEFAULT {QuoteDefaultValue(column.Default)}";
            }
            return "";
        }

        private static string GetNullabilityStatement(ColumnInfo column) =>
            column.Nullable switch
            {
                true => "NULL",
                false => "NOT NULL",
            };

        private static string QuoteDefaultValue(object value)
        {
            return value switch
            {
                string => $"'{value}'",
                int => $"{value}",
                byte[] => $"{ToHex((byte[])value)}",
                _ => throw new InvalidOperationException($"Invalid value type: '{value.GetType()}'")
            };

            static string ToHex(byte[] val) => "0x" + BitConverter.ToString(val).Replace("-", "");
        }
    }
}
