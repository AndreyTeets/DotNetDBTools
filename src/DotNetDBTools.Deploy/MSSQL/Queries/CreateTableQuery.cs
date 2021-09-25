using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;
using static DotNetDBTools.Deploy.MSSQL.MSSQLQueriesHelper;
using static DotNetDBTools.Deploy.MSSQL.MSSQLSqlTypeMapper;

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

            tableDefinitions.AddRange(table.Columns.Select(c =>
$@"    {c.Name} {GetSqlType(c.DataType)}{GetIdentityStatement(c)} {GetNullabilityStatement(c)}{GetDefaultValStatement(c)}"));

            if (table.PrimaryKey is not null)
            {
                tableDefinitions.Add(
$@"    CONSTRAINT {table.PrimaryKey.Name} PRIMARY KEY ({string.Join(", ", table.PrimaryKey.Columns)})");
            }

            tableDefinitions.AddRange(table.UniqueConstraints.Select(uc =>
$@"    CONSTRAINT {uc.Name} UNIQUE ({string.Join(", ", uc.Columns)})"));

            return string.Join(",\n", tableDefinitions);
        }

        private static string GetDefaultValStatement(ColumnInfo column)
        {
            if (column.Default is not null)
            {
                return $" CONSTRAINT {column.DefaultConstraintName} DEFAULT {QuoteDefaultValue(column.Default)}";
            }
            return "";
        }
    }
}
