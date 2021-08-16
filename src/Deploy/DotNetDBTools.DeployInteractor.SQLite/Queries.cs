using DotNetDBTools.Models.SQLite;
using System.Collections.Generic;
using System.Linq;

namespace DotNetDBTools.DeployInteractor.SQLite
{
    internal static class Queries
    {
        public static readonly string DatabaseExists =
$@"SELECT
    true
FROM sqlite_master 
WHERE type = 'table' AND name = '{DNDBTSysTables.DNDBTDbObjects}';";

        public static readonly string CreateEmptyDatabase =
$@"CREATE TABLE {DNDBTSysTables.DNDBTDbObjects}
(
    {DNDBTSysTables.DNDBTDbObjects.ID} BLOB PRIMARY KEY,
    {DNDBTSysTables.DNDBTDbObjects.Type} TEXT NOT NULL,
    {DNDBTSysTables.DNDBTDbObjects.Name} TEXT NOT NULL,
    {DNDBTSysTables.DNDBTDbObjects.Metadata} TEXT NOT NULL
) WITHOUT ROWID;";

        public static readonly string GetExistingTables =
$@"SELECT
    {DNDBTSysTables.DNDBTDbObjects.Metadata}
FROM {DNDBTSysTables.DNDBTDbObjects}
WHERE {DNDBTSysTables.DNDBTDbObjects.Type} = '{SQLiteDbObjectsTypes.Table}';";

        public static string CreateTable(SQLiteTableInfo table)
        {
            List<string> tableDefinitions = new();
            tableDefinitions.AddRange(table.Columns.Select(column => $@"    {column.Name} {column.DataType}"));
            tableDefinitions.AddRange(table.ForeignKeys.Select(fk => $@"    CONSTRAINT {fk.Name} FOREIGN KEY ({string.Join(",", fk.ThisColumnNames)}) REFERENCES {fk.ForeignTableName}({string.Join(",", fk.ForeignColumnNames)})"));

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
    @{DNDBTSysTables.DNDBTDbObjects.Metadata}
);

CREATE TABLE {table.Name}
(
{string.Join(",\n", tableDefinitions)}
);";

            return query;
        }
    }
}
