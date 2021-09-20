using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
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
$@"    {column.Name} {MSSQLSqlTypeMapper.GetSqlType(column.DataType)} UNIQUE"));

            tableDefinitions.AddRange(table.ForeignKeys.Select(fk =>
$@"    CONSTRAINT {fk.Name} FOREIGN KEY ({string.Join(",", fk.ThisColumnNames)}) REFERENCES {fk.ForeignTableName}({string.Join(",", fk.ForeignColumnNames)})"));

            return string.Join(",\n", tableDefinitions);
        }
    }
}
