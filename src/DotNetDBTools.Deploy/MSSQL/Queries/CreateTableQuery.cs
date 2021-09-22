using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL.Queries
{
    internal class CreateTableQuery : IQuery
    {
        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        public CreateTableQuery(MSSQLTableInfo table)
        {
            _sql = GetSql(table);
            _parameters = new List<QueryParameter>();
        }

        private static string GetSql(MSSQLTableInfo table)
        {
            string query =
$@"CREATE TABLE {table.Name}
(
{GetTableDefinitionsText(table)}
);";

            return query;
        }

        private static string GetTableDefinitionsText(MSSQLTableInfo table)
        {
            List<string> tableDefinitions = new();

            tableDefinitions.AddRange(table.Columns.Select(column =>
$@"    {column.Name} {MSSQLSqlTypeMapper.GetSqlType(column.DataType)} {GetNullability(column)} {GetDefaultability(column)}"));

            tableDefinitions.AddRange(table.Columns.Where(column => column.Unique).Select(column =>
$@"    CONSTRAINT UQ_{table.Name}_{column.Name} UNIQUE ({column.Name})"));

            tableDefinitions.AddRange(table.ForeignKeys.Select(fk =>
$@"    CONSTRAINT {fk.Name} FOREIGN KEY ({string.Join(",", fk.ThisColumnNames)})
        REFERENCES {fk.ForeignTableName}({string.Join(",", fk.ForeignColumnNames)})
        ON UPDATE {MapActionName(fk.OnUpdate)} ON DELETE {MapActionName(fk.OnDelete)}"));

            return string.Join(",\n", tableDefinitions);
        }

        private static string GetNullability(ColumnInfo column) =>
            column.Nullable switch
            {
                true => "NULL",
                false => "NOT NULL",
            };

        private static string GetDefaultability(ColumnInfo column)
        {
            if (column.Default is not null)
            {
                return $"DEFAULT {QuoteValue(column.Default)}";
            }
            return "";
        }

        private static string QuoteValue(object value)
        {
            return value switch
            {
                MSSQLDefaultValueAsFunction => $"{((MSSQLDefaultValueAsFunction)value).FunctionText}",
                string => $"'{value}'",
                int => $"{value}",
                byte[] => $"{ToHex((byte[])value)}",
                _ => throw new InvalidOperationException($"Invalid value type: '{value.GetType()}'")
            };

            static string ToHex(byte[] val) => "0x" + BitConverter.ToString(val).Replace("-", "");
        }

        private static string MapActionName(string modelActionName) =>
            modelActionName switch
            {
                "NoAction" => "NO ACTION",
                "Cascade" => "CASCADE",
                _ => throw new InvalidOperationException($"Invalid modelActionName: '{modelActionName}'")
            };
    }
}
