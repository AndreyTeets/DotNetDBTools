using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Deploy.SQLite.Queries
{
    internal class CreateTableQuery : IQuery
    {
        private const string MetadataParameterName = "@Metadata";
        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        public CreateTableQuery(SQLiteTableInfo table, string metadataParameterValue)
        {
            _sql = GetSql(table);
            _parameters = new List<QueryParameter>
            {
                new QueryParameter(MetadataParameterName, metadataParameterValue),
            };
        }

        private static string GetSql(SQLiteTableInfo table)
        {
            string query =
$@"INSERT INTO {DNDBTSysTables.DNDBTDbObjects}
(
    {DNDBTSysTables.DNDBTDbObjects.ID},
    {DNDBTSysTables.DNDBTDbObjects.Type},
    {DNDBTSysTables.DNDBTDbObjects.Name},
    {DNDBTSysTables.DNDBTDbObjects.Metadata}
)
VALUES
(
    '{table.ID}',
    '{SQLiteDbObjectsTypes.Table}',
    '{table.Name}',
    {MetadataParameterName}
);

CREATE TABLE {table.Name}
(
{GetTableDefinitionsText(table)}
);";

            return query;
        }

        private static string GetTableDefinitionsText(SQLiteTableInfo table)
        {
            List<string> tableDefinitions = new();

            tableDefinitions.AddRange(table.Columns.Select(column =>
$@"    {column.Name} {SQLiteSqlTypeMapper.GetSqlType(column.DataType)} UNIQUE"));

            tableDefinitions.AddRange(table.ForeignKeys.Select(fk =>
$@"    CONSTRAINT {fk.Name} FOREIGN KEY ({string.Join(",", fk.ThisColumnNames)}) REFERENCES {fk.ForeignTableName}({string.Join(",", fk.ForeignColumnNames)})"));

            return string.Join(",\n", tableDefinitions);
        }
    }
}
