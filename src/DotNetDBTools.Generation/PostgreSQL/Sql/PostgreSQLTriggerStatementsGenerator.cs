using DotNetDBTools.Generation.Core;
using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Generation.PostgreSQL.Sql;

internal class PostgreSQLTriggerStatementsGenerator : StatementsGenerator<PostgreSQLTrigger>
{
    protected override string GetCreateSqlImpl(PostgreSQLTrigger trigger)
    {
        string res =
$@"{GetIdDeclarationText(trigger, 0)}{trigger.GetCode()}";

        return res;
    }

    protected override string GetDropSqlImpl(PostgreSQLTrigger trigger)
    {
        return $@"DROP TRIGGER ""{trigger.Name}"" ON ""{trigger.TableName}"";";
    }
}
