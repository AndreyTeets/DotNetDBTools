using DotNetDBTools.Generation.Core;
using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Generation.MySQL.Sql;

internal class MySQLViewStatementsGenerator : StatementsGenerator<MySQLView>
{
    protected override string GetCreateSqlImpl(MySQLView view)
    {
        return $"{GetIdDeclarationText(view, 0)}{view.GetCreateStatement().AppendSemicolonIfAbsent()}";
    }

    protected override string GetDropSqlImpl(MySQLView view)
    {
        return $"DROP VIEW `{view.Name}`;";
    }
}
