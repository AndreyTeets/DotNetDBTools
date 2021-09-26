using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.SQLite;
using static DotNetDBTools.Deploy.SQLite.SQLiteQueriesHelper;

namespace DotNetDBTools.Deploy.SQLite.Queries
{
    internal class AlterTableQuery : IQuery
    {
        private const string DNDBTTempPrefix = "DNDBT_";
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
