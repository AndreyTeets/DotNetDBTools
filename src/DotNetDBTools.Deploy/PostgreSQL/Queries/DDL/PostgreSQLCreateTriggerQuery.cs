using DotNetDBTools.Deploy.Common.Queries.DDL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;

internal class PostgreSQLCreateTriggerQuery : CreateTriggerQuery
{
    public PostgreSQLCreateTriggerQuery(Trigger trigger)
        : base(trigger) { }

    protected override string GetSql(Trigger trigger)
    {
        string query =
$@"{trigger.GetCode()}";

        return query;
    }
}
