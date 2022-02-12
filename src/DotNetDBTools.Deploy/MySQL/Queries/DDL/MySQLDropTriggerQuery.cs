using DotNetDBTools.Deploy.Common.Queries.DDL;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MySQL.Queries.DDL;

internal class MySQLDropTriggerQuery : DropTriggerQuery
{
    public MySQLDropTriggerQuery(Trigger trigger, Table table)
        : base(trigger, table) { }

    protected override string GetSql(Trigger trigger, Table table)
    {
        string query =
$@"DROP TRIGGER `{trigger.Name}`;";

        return query;
    }
}
