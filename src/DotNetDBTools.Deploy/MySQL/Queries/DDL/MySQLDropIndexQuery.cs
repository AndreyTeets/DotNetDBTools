using DotNetDBTools.Deploy.Common.Queries.DDL;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MySQL.Queries.DDL;

internal class MySQLDropIndexQuery : DropIndexQuery
{
    public MySQLDropIndexQuery(Index index, Table table)
        : base(index, table) { }

    protected override string GetSql(Index index, Table table)
    {
        string query =
$@"DROP INDEX `{index.Name}` ON `{table.Name}`;";

        return query;
    }
}
