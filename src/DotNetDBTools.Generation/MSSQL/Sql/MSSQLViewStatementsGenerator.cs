using DotNetDBTools.Generation.Core;
using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Generation.MSSQL;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Generation.MSSQL.Sql;

internal class MSSQLViewStatementsGenerator : StatementsGenerator<MSSQLView>
{
    protected override string GetCreateSqlImpl(MSSQLView view)
    {
        return $"{GetIdDeclarationText(view, 0)}{view.GetCreateStatement()}";
    }

    protected override string GetDropSqlImpl(MSSQLView view)
    {
        return $"DROP VIEW [{view.Name}];";
    }
}
