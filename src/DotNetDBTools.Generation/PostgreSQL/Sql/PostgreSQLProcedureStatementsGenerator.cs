using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Generation.PostgreSQL.Sql;

internal class PostgreSQLProcedureStatementsGenerator : StatementsGenerator<PostgreSQLProcedure>
{
    protected override string GetCreateSqlImpl(PostgreSQLProcedure procedure)
    {
        return $"{GetIdDeclarationText(procedure, 0)}{procedure.GetCreateStatement()}";
    }

    protected override string GetDropSqlImpl(PostgreSQLProcedure procedure)
    {
        return $@"DROP PROCEDURE ""{procedure.Name}"";";
    }
}
