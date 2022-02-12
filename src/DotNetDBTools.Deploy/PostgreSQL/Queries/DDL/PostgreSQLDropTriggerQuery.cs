using DotNetDBTools.Deploy.Common.Queries.DDL;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;

internal class PostgreSQLDropTriggerQuery : DropTriggerQuery
{
    public PostgreSQLDropTriggerQuery(Trigger trigger, Table table)
        : base(trigger, table) { }

    protected override string GetSql(Trigger trigger, Table table)
    {
        string query =
$@"DROP TRIGGER ""{trigger.Name}"" ON ""{table.Name}"";";

        return query;
    }
}
