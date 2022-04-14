using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Deploy.SQLite.SQLiteQueriesHelper;

namespace DotNetDBTools.Deploy.SQLite.Queries.DDL;

internal class SQLiteCreateTableQuery : CreateTableQuery
{
    public SQLiteCreateTableQuery(Table table)
        : base(table) { }

    protected override string GetSql(Table table)
    {
        List<string> createTriggerStatements = new();
        foreach (Trigger trigger in table.Triggers)
        {
            createTriggerStatements.Add(
$@"{trigger.GetCode().AppendSemicolonIfAbsent()}");
        }

        List<string> createIndexStatements = new();
        foreach (Index index in table.Indexes)
        {
            createIndexStatements.Add(
$@"CREATE{GetUniqueStatement(index.Unique)} INDEX [{index.Name}]
ON [{table.Name}] ({string.Join(", ", index.Columns.Select(x => $@"[{x}]"))});");
        }

        string createTableStatement =
$@"CREATE TABLE [{table.Name}]
(
{GetTableDefinitionsText(table)}
);";

        StringBuilder sb = new();

        sb.Append(createTableStatement);

        if (createTriggerStatements.Any())
            sb.AppendLine().AppendLine().Append(string.Join("\n", createTriggerStatements));
        if (createIndexStatements.Any())
            sb.AppendLine().AppendLine().Append(string.Join("\n", createIndexStatements));

        return sb.ToString();
    }
}
