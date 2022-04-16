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
$@"DROP INDEX [{index.Name}];");
        }

        List<string> dropTriggerStatements = new();
        foreach (Trigger trigger in tableDiff.TriggersToDrop)
        {
            dropTriggerStatements.Add(
$@"DROP TRIGGER [{trigger.Name}];");
        }

        List<string> createTriggerStatements = new();
        foreach (Trigger trigger in tableDiff.TriggersToCreate)
        {
            createTriggerStatements.Add(
$@"{trigger.GetCode().AppendSemicolonIfAbsent()}");
        }

        List<string> createIndexStatements = new();
        foreach (Index index in tableDiff.IndexesToCreate)
        {
            createIndexStatements.Add(
$@"CREATE{GetUniqueStatement(index.Unique)} INDEX [{index.Name}]
ON [{tableDiff.NewTable.Name}] ({string.Join(", ", index.Columns.Select(x => $@"[{x}]"))});");
        }

        string alterTableStatement =
$@"CREATE TABLE [{DNDBTTempPrefix}{tableDiff.NewTable.Name}]
(
{GetTableDefinitionsText((SQLiteTable)tableDiff.NewTable)}
);

INSERT INTO [{DNDBTTempPrefix}{tableDiff.NewTable.Name}]
(
{GetCommonColumnsNewNamesText(tableDiff)}
)
SELECT
{GetCommonColumnsOldNamesText(tableDiff)}
FROM [{tableDiff.OldTable.Name}];

DROP TABLE [{tableDiff.OldTable.Name}];

ALTER TABLE [{DNDBTTempPrefix}{tableDiff.NewTable.Name}] RENAME TO [{tableDiff.NewTable.Name}];";

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

    private static string GetCommonColumnsNewNamesText(TableDiff tableDiff)
    {
        IEnumerable<string> commonNewOldColumnsNames = tableDiff.NewTable.Columns.Select(x => x.Name)
            .Except(tableDiff.AddedColumns.Select(x => x.Name));
        return string.Join(",\n", commonNewOldColumnsNames.Select(x => $@"    [{x}]"));
    }

    private static string GetCommonColumnsOldNamesText(TableDiff tableDiff)
    {
        IEnumerable<string> commonNewOldColumnsNames = tableDiff.OldTable.Columns.Select(x => x.Name)
            .Except(tableDiff.RemovedColumns.Select(x => x.Name));
        return string.Join(",\n", commonNewOldColumnsNames.Select(x => $@"    [{x}]"));
    }
}
