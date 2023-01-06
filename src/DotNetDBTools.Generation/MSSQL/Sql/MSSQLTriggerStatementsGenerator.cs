using DotNetDBTools.Generation.Core;
using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Generation.MSSQL.Sql;

internal class MSSQLTriggerStatementsGenerator : StatementsGenerator<MSSQLTrigger>
{
    protected override string GetCreateSqlImpl(MSSQLTrigger trigger)
    {
        string res =
$@"{GetIdDeclarationText(trigger, 0)}{trigger.GetCode()}";

        return res;
    }

    protected override string GetDropSqlImpl(MSSQLTrigger trigger)
    {
        return $"DROP TRIGGER [{trigger.Name}];";
    }
}
