using DotNetDBTools.Deploy.Common.Queries.DDL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MySQL.Queries.DDL;

internal class MySQLCreateTriggerQuery : CreateTriggerQuery
{
    public MySQLCreateTriggerQuery(Trigger trigger)
        : base(trigger) { }

    protected override string GetSql(Trigger trigger)
    {
        string query =
$@"{trigger.GetCode().AppendSemicolonIfAbsent()}";

        return query;
    }
}
