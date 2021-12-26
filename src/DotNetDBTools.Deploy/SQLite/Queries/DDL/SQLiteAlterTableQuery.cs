using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;
using static DotNetDBTools.Deploy.SQLite.SQLiteQueriesHelper;

namespace DotNetDBTools.Deploy.SQLite.Queries.DDL
{
    internal class SQLiteAlterTableQuery : AlterTableQuery
    {
        private const string DNDBTTempPrefix = "_DNDBTTemp_";

        public SQLiteAlterTableQuery(TableDiff tableDiff)
            : base(tableDiff) { }

        protected override string GetSql(TableDiff tableDiff)
        {
            foreach (Index index in tableDiff.IndexesToDrop)
            {
                string _ =
$@"DROP INDEX {index.Name};";
            }

            foreach (Trigger trigger in tableDiff.TriggersToDrop)
            {
                string _ =
$@"DROP TRIGGER {trigger.Name};";
            }

            string query =
$@"CREATE TABLE {DNDBTTempPrefix}{tableDiff.NewTable.Name}
(
{GetTableDefinitionsText((SQLiteTable)tableDiff.NewTable)}
);

INSERT INTO  {DNDBTTempPrefix}{tableDiff.NewTable.Name}({GetChangedColumnsNewNamesText(tableDiff)})
SELECT {GetChangedColumnsOldNamesText(tableDiff)}
FROM {tableDiff.OldTable.Name};

DROP TABLE {tableDiff.OldTable.Name};

ALTER TABLE {DNDBTTempPrefix}{tableDiff.NewTable.Name} RENAME TO {tableDiff.NewTable.Name};";

            foreach (Index index in tableDiff.IndexesToCreate)
            {
                string _ =
$@"CREATE INDEX {index.Name}
ON {tableDiff.NewTable.Name} ({string.Join(", ", index.Columns)});";
            }

            foreach (Trigger trigger in tableDiff.TriggersToCreate)
            {
                string _ =
$@"{trigger.CodePiece}";
            }

            return query;
        }

        private static string GetChangedColumnsNewNamesText(TableDiff tableDiff)
        {
            IEnumerable<string> columnsNames = tableDiff.ChangedColumns.Select(columnDiff => columnDiff.NewColumn.Name);
            return string.Join(", ", columnsNames);
        }

        private static string GetChangedColumnsOldNamesText(TableDiff tableDiff)
        {
            IEnumerable<string> columnsNames = tableDiff.ChangedColumns.Select(columnDiff => columnDiff.OldColumn.Name);
            return string.Join(", ", columnsNames);
        }
    }
}
