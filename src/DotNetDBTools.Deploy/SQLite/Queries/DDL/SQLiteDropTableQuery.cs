using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.SQLite.Queries.DDL;

internal class SQLiteDropTableQuery : DropTableQuery
{
    public SQLiteDropTableQuery(Table table)
        : base(table) { }

    protected override string GetSql(Table table)
    {
        string query =
$@"DROP TABLE {table.Name};";

        return query;
    }
}
