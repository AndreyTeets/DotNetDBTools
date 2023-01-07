using DotNetDBTools.Generation.Core;
using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Generation.MySQL.Sql;

internal class MySQLTriggerStatementsGenerator : StatementsGenerator<MySQLTrigger>
{
    protected override string GetCreateSqlImpl(MySQLTrigger trigger)
    {
        string res =
$@"{GetIdDeclarationText(trigger, 0)}{trigger.GetCreateStatement().AppendSemicolonIfAbsent()}";

        return res;
    }

    protected override string GetDropSqlImpl(MySQLTrigger trigger)
    {
        return $"DROP TRIGGER `{trigger.Name}`;";
    }
}
