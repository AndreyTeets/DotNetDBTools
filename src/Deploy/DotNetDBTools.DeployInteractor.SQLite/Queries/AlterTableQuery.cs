using DotNetDBTools.Models.SQLite;
using System.Collections.Generic;
using System.Linq;

namespace DotNetDBTools.DeployInteractor.SQLite.Queries
{
    internal class AlterTableQuery : IQuery
    {
        private const string DNDBTTempPrefix = "DNDBT_";
        private const string NewTableMetadataParameterName = "@NewTableMetadata";
        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        public AlterTableQuery(SQLiteTableDiff tableDiff, string newTableMetadataParameterValue)
        {
            _sql = GetSql(tableDiff);
            _parameters = new List<QueryParameter>
            {
                new QueryParameter(NewTableMetadataParameterName, newTableMetadataParameterValue),
            };
        }

        private static string GetSql(SQLiteTableDiff tableDiff)
        {
            string query =
$@"PRAGMA foreign_keys=off;
BEGIN TRANSACTION;

CREATE TABLE {DNDBTTempPrefix}{tableDiff.NewTable.Name}
(
{GetTableDefinitionsText(tableDiff.NewTable)}
);

INSERT INTO  {DNDBTTempPrefix}{tableDiff.NewTable.Name}({GetChangedColumnsNewNamesText(tableDiff)})
SELECT {GetChangedColumnsOldNamesText(tableDiff)}
FROM {tableDiff.OldTable.Name};

DROP TABLE {tableDiff.OldTable.Name};

ALTER TABLE {DNDBTTempPrefix}{tableDiff.NewTable.Name} RENAME TO {tableDiff.NewTable.Name};

COMMIT TRANSACTION;
PRAGMA foreign_keys=on;

UPDATE {DNDBTSysTables.DNDBTDbObjects} SET
    {DNDBTSysTables.DNDBTDbObjects.Name} = '{tableDiff.NewTable.Name}',
    {DNDBTSysTables.DNDBTDbObjects.Metadata} = {NewTableMetadataParameterName}
WHERE {DNDBTSysTables.DNDBTDbObjects.ID} = '{tableDiff.NewTable.ID}';";

            return query;
        }

        private static string GetChangedColumnsNewNamesText(SQLiteTableDiff tableDiff)
        {
            List<string> commonColumnsNames = new();
            IEnumerable<string> columnsNames = tableDiff.ChangedColumns.Select(columnDiff => columnDiff.NewColumn.Name);
            return string.Join(", ", columnsNames);
        }

        private static string GetChangedColumnsOldNamesText(SQLiteTableDiff tableDiff)
        {
            List<string> commonColumnsNames = new();
            IEnumerable<string> columnsNames = tableDiff.ChangedColumns.Select(columnDiff => columnDiff.OldColumn.Name);
            return string.Join(", ", columnsNames);
        }

        private static string GetTableDefinitionsText(SQLiteTableInfo table)
        {
            List<string> tableDefinitions = new();

            tableDefinitions.AddRange(table.Columns.Select(column =>
$@"    {column.Name} {column.DataType} UNIQUE"));

            tableDefinitions.AddRange(table.ForeignKeys.Select(fk =>
$@"    CONSTRAINT {fk.Name} FOREIGN KEY ({string.Join(",", fk.ThisColumnNames)}) REFERENCES {fk.ForeignTableName}({string.Join(",", fk.ForeignColumnNames)})"));

            return string.Join(",\n", tableDefinitions);
        }
    }
}
