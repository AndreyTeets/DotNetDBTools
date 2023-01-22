using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Generation.PostgreSQL;

public static class PostgreSQLHelperExtensions
{
    public static string GetDefault(this PostgreSQLDomainType type) => type.Default?.Code;
    public static string GetCreateStatement(this PostgreSQLFunction func) => func.CreateStatement.Code;
    public static string GetCreateStatement(this PostgreSQLProcedure proc) => proc.CreateStatement.Code;

    public static string InsideDoPlPgSqlBlock(this string innerCode)
    {
        string res =
$@"DO $DNDBTPlPgSqlBlock$
BEGIN
{innerCode}
END;
$DNDBTPlPgSqlBlock$";

        return res;
    }
}
