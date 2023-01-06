using DotNetDBTools.Generation.Core;
using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Generation.SQLite.Sql;

internal class SQLiteViewStatementsGenerator : StatementsGenerator<SQLiteView>
{
    protected override string GetCreateSqlImpl(SQLiteView view)
    {
        return $"{GetIdDeclarationText(view, 0)}{view.GetCode().AppendSemicolonIfAbsent()}";
    }

    protected override string GetDropSqlImpl(SQLiteView view)
    {
        return $"DROP VIEW [{view.Name}];";
    }
}
