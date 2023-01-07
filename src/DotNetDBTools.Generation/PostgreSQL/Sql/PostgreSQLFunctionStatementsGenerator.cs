using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Generation.PostgreSQL.Sql;

internal class PostgreSQLFunctionStatementsGenerator : StatementsGenerator<PostgreSQLFunction>
{
    protected override string GetCreateSqlImpl(PostgreSQLFunction function)
    {
        return $"{GetIdDeclarationText(function, 0)}{function.GetCreateStatement()}";
    }

    protected override string GetDropSqlImpl(PostgreSQLFunction function)
    {
        return $@"DROP FUNCTION ""{function.Name}"";";
    }
}
