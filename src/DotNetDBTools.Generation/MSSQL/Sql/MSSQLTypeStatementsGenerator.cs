using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Models.MSSQL.UserDefinedTypes;

namespace DotNetDBTools.Generation.MSSQL.Sql;

internal class MSSQLTypeStatementsGenerator : StatementsGenerator<MSSQLUserDefinedType>
{
    protected override string GetCreateSqlImpl(MSSQLUserDefinedType type)
    {
        string res =
$@"{GetIdDeclarationText(type, 0)}CREATE TYPE [{type.Name}] FROM {type.UnderlyingType.Name};";

        return res;
    }

    protected override string GetDropSqlImpl(MSSQLUserDefinedType type)
    {
        return $"DROP TYPE [{type.Name}];";
    }
}
