using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;
using static DotNetDBTools.Deploy.SQLite.SQLiteQueriesHelper;

namespace DotNetDBTools.Deploy.SQLite.Queries.DDL;

internal class SQLiteAlterTableQuery : AlterTableQuery
{
    private const string DNDBTTempPrefix = "_DNDBTTemp_";

    public SQLiteAlterTableQuery(TableDiff tableDiff)
        : base(tableDiff) { }

    protected override string GetSql(TableDiff tableDiff)
    {
        List<string> dropIndexStatements = new();
        foreach (Index index in tableDiff.IndexesToDrop)
        {
            dropIndexStatements.Add(
$@"DROP INDEX {index.Name};");
        }

        List<string> dropTriggerStatements = new();
        foreach (Trigger trigger in tableDiff.TriggersToDrop)
        {
            dropTriggerStatements.Add(
$@"DROP TRIGGER {trigger.Name};");
        }

        List<string> createTriggerStatements = new();
        foreach (Trigger trigger in tableDiff.TriggersToCreate)
        {
            createTriggerStatements.Add(
$@"{AppendSemicolonIfAbsent(trigger.GetCode())}");
        }

        List<string> createIndexStatements = new();
        foreach (Index index in tableDiff.IndexesToCreate)
        {
            createIndexStatements.Add(
$@"CREATE{GetUniqueStatement(index.Unique)} INDEX {index.Name}
ON {tableDiff.NewTable.Name} ({string.Join(", ", index.Columns)});");
        }

        string alterTableStatement =
$@"CREATE TABLE {DNDBTTempPrefix}{tableDiff.NewTable.Name}
(
{GetTableDefinitionsText((SQLiteTable)tableDiff.NewTable)}
);

INSERT INTO  {DNDBTTempPrefix}{tableDiff.NewTable.Name}({GetChangedColumnsNewNamesText(tableDiff)})
SELECT {GetChangedColumnsOldNamesText(tableDiff)}
FROM {tableDiff.OldTable.Name};

DROP TABLE {tableDiff.OldTable.Name};

ALTER TABLE {DNDBTTempPrefix}{tableDiff.NewTable.Name} RENAME TO {tableDiff.NewTable.Name};";

        StringBuilder sb = new();

        if (dropIndexStatements.Any())
            sb.Append(string.Join("\n", dropIndexStatements)).AppendLine().AppendLine();
        if (dropTriggerStatements.Any())
            sb.Append(string.Join("\n", dropTriggerStatements)).AppendLine().AppendLine();

        sb.Append(alterTableStatement);

        if (createTriggerStatements.Any())
            sb.AppendLine().AppendLine().Append(string.Join("\n", createTriggerStatements));
        if (createIndexStatements.Any())
            sb.AppendLine().AppendLine().Append(string.Join("\n", createIndexStatements));

        return sb.ToString();
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
