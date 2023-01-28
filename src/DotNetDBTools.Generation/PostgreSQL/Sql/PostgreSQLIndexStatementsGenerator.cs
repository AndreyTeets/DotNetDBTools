using System.Linq;
using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Generation.PostgreSQL.Sql;

internal class PostgreSQLIndexStatementsGenerator : StatementsGenerator<PostgreSQLIndex>
{
    protected override string GetCreateSqlImpl(PostgreSQLIndex index)
    {
        string res =
$@"{GetIdDeclarationText(index, 0)}CREATE{Statements.Unique(index)} INDEX ""{index.Name}""
    ON ""{index.Parent.Name}"" USING {index.Method} ({Statements.ColsOrExpr(index)}){Statements.Include(index)};";

        return res;
    }

    protected override string GetDropSqlImpl(PostgreSQLIndex index)
    {
        return $@"DROP INDEX ""{index.Name}"";";
    }

    private static class Statements
    {
        public static string Unique(Index index) =>
index.Unique ? " UNIQUE" : ""
            ;

        public static string ColsOrExpr(PostgreSQLIndex index)
        {
            if (index.Expression is not null)
                return index.Expression.Code;
            else
                return string.Join(", ", index.Columns.Select(x => $@"""{x}"""));
        }

        public static string Include(PostgreSQLIndex index)
        {
            if (index.IncludeColumns.Count() > 0)
                return $"\n    INCLUDE ({string.Join(", ", index.IncludeColumns.Select(x => $@"""{x}"""))})";
            else
                return "";
        }
    }
}
