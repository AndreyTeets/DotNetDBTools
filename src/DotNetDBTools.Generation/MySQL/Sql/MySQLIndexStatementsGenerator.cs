using System.Linq;
using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Generation.MySQL.Sql;

internal class MySQLIndexStatementsGenerator : StatementsGenerator<MySQLIndex>
{
    protected override string GetCreateSqlImpl(MySQLIndex index)
    {
        string res =
$@"{GetIdDeclarationText(index, 0)}CREATE{Statements.Unique(index)} INDEX `{index.Name}`
    ON `{index.Parent.Name}` ({string.Join(", ", index.Columns.Select(x => $@"`{x}`"))});";

        return res;
    }

    protected override string GetDropSqlImpl(MySQLIndex index)
    {
        return $"DROP INDEX `{index.Name}` ON `{index.Parent.Name}`;";
    }

    private static class Statements
    {
        public static string Unique(Index index) =>
index.Unique ? " UNIQUE" : ""
            ;
    }
}
