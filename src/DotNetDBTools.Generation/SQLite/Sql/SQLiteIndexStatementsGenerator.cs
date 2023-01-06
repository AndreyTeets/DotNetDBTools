using System.Linq;
using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Generation.SQLite.Sql;

internal class SQLiteIndexStatementsGenerator : StatementsGenerator<SQLiteIndex>
{
    protected override string GetCreateSqlImpl(SQLiteIndex index)
    {
        string res =
$@"{GetIdDeclarationText(index, 0)}CREATE{Statements.Unique(index)} INDEX [{index.Name}]
    ON [{index.TableName}] ({string.Join(", ", index.Columns.Select(x => $@"[{x}]"))});";

        return res;
    }

    protected override string GetDropSqlImpl(SQLiteIndex index)
    {
        return $"DROP INDEX [{index.Name}];";
    }

    private static class Statements
    {
        public static string Unique(Index index) =>
index.Unique ? " UNIQUE" : ""
            ;
    }
}
