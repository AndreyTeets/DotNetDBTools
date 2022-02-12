using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;

internal class PostgreSQLDropTableQuery : DropTableQuery
{
    public PostgreSQLDropTableQuery(Table table)
        : base(table) { }

    protected override string GetSql(Table table)
    {
        string query =
$@"DROP TABLE ""{table.Name}"";";

        return query;
    }
}
