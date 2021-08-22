using DotNetDBTools.Models.SQLite;
using System.Collections.Generic;
using System.Linq;

namespace DotNetDBTools.DeployInteractor.SQLite.Queries
{
    internal static class CreateTableQuery
    {
        public static class Parameters
        {
            public const string Metadata = "@Metadata";
        }

        public static string Sql(SQLiteTableInfo table)
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
    {Parameters.Metadata}
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
$@"    {column.Name} {column.DataType}"));

            tableDefinitions.AddRange(table.ForeignKeys.Select(fk =>
$@"    CONSTRAINT {fk.Name} FOREIGN KEY ({string.Join(",", fk.ThisColumnNames)}) REFERENCES {fk.ForeignTableName}({string.Join(",", fk.ForeignColumnNames)})"));

            return string.Join(",\n", tableDefinitions);
        }
    }
}
