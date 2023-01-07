using DotNetDBTools.Generation.Core;
using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Generation.SQLite.Sql;

internal class SQLiteTriggerStatementsGenerator : StatementsGenerator<SQLiteTrigger>
{
    protected override string GetCreateSqlImpl(SQLiteTrigger trigger)
    {
        string res =
$"{GetIdDeclarationText(trigger, 0)}{trigger.GetCreateStatement().AppendSemicolonIfAbsent()}";

        return res;
    }

    protected override string GetDropSqlImpl(SQLiteTrigger trigger)
    {
        return $"DROP TRIGGER [{trigger.Name}];";
    }
}
