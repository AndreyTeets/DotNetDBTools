using System.Linq;
using DotNetDBTools.Deploy.Common.Queries.DDL;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;

internal class PostgreSQLCreateIndexQuery : CreateIndexQuery
{
    public PostgreSQLCreateIndexQuery(Index index, Table table)
        : base(index, table) { }

    protected override string GetSql(Index index, Table table)
    {
        string query =
$@"CREATE{GetUniqueStatement(index.Unique)} INDEX ""{index.Name}""
ON ""{table.Name}"" ({string.Join(", ", index.Columns.Select(x => $@"""{x}"""))});";

        return query;
    }

    private static string GetUniqueStatement(bool unique)
    {
        if (unique)
            return " UNIQUE";
        else
            return "";
    }
}
