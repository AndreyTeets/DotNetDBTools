using System.Linq;
using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Generation.MSSQL.Sql;

internal class MSSQLIndexStatementsGenerator : StatementsGenerator<MSSQLIndex>
{
    protected override string GetCreateSqlImpl(MSSQLIndex index)
    {
        string res =
$@"{GetIdDeclarationText(index, 0)}CREATE{Statements.Unique(index)} INDEX [{index.Name}]
    ON [{index.Parent.Name}] ({string.Join(", ", index.Columns.Select(x => $@"[{x}]"))});";

        return res;
    }

    protected override string GetDropSqlImpl(MSSQLIndex index)
    {
        return $"DROP INDEX [{index.Name}] ON [{index.Parent.Name}];";
    }

    private static class Statements
    {
        public static string Unique(Index index) =>
index.Unique ? " UNIQUE" : ""
            ;
    }
}
