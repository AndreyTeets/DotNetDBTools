using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;
using static DotNetDBTools.Deploy.SQLite.SQLiteQueriesHelper;

namespace DotNetDBTools.Deploy.SQLite.Queries
{
    internal class AlterTableQuery : IQuery
    {
        private const string DNDBTTempPrefix = "_DNDBTTemp_";
        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        public AlterTableQuery(SQLiteTableDiff tableDiff)
        {
            _sql = GetSql(tableDiff);
            _parameters = new List<QueryParameter>();
        }

        private static string GetSql(SQLiteTableDiff tableDiff)
        {
            foreach (IndexInfo index in tableDiff.IndexesToDrop)
            {
                string _ =
$@"DROP INDEX {index.Name};";
            }

            foreach (TriggerInfo trigger in tableDiff.TriggersToDrop)
            {
                string _ =
$@"DROP TRIGGER {trigger.Name};";
            }

            string query =
$@"CREATE TABLE {DNDBTTempPrefix}{tableDiff.NewTable.Name}
(
{GetTableDefinitionsText((SQLiteTableInfo)tableDiff.NewTable)}
);

INSERT INTO  {DNDBTTempPrefix}{tableDiff.NewTable.Name}({GetChangedColumnsNewNamesText(tableDiff)})
SELECT {GetChangedColumnsOldNamesText(tableDiff)}
FROM {tableDiff.OldTable.Name};

DROP TABLE {tableDiff.OldTable.Name};

ALTER TABLE {DNDBTTempPrefix}{tableDiff.NewTable.Name} RENAME TO {tableDiff.NewTable.Name};";

            foreach (IndexInfo index in tableDiff.IndexesToCreate)
            {
                string _ =
$@"CREATE INDEX {index.Name}
ON {tableDiff.NewTable.Name} ({string.Join(", ", index.Columns)});";
            }

            foreach (TriggerInfo trigger in tableDiff.TriggersToCreate)
            {
                string _ =
$@"{trigger.Code}";
            }

            return query;
        }

        private static string GetChangedColumnsNewNamesText(SQLiteTableDiff tableDiff)
        {
            IEnumerable<string> columnsNames = tableDiff.ChangedColumns.Select(columnDiff => columnDiff.NewColumn.Name);
            return string.Join(", ", columnsNames);
        }

        private static string GetChangedColumnsOldNamesText(SQLiteTableDiff tableDiff)
        {
            IEnumerable<string> columnsNames = tableDiff.ChangedColumns.Select(columnDiff => columnDiff.OldColumn.Name);
            return string.Join(", ", columnsNames);
        }
    }
}
