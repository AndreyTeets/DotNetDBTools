using DotNetDBTools.Generation.Core;
using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Generation.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Generation.PostgreSQL.Sql;

internal class PostgreSQLViewStatementsGenerator : StatementsGenerator<PostgreSQLView>
{
    protected override string GetCreateSqlImpl(PostgreSQLView view)
    {
        return $"{GetIdDeclarationText(view, 0)}{view.GetCode()}";
    }

    protected override string GetDropSqlImpl(PostgreSQLView view)
    {
        return $@"DROP VIEW ""{view.Name}"";";
    }
}
