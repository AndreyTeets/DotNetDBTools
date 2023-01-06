using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Generation.PostgreSQL;

public static class PostgreSQLHelperExtensions
{
    public static string GetQuotedName(this DataType type)
    {
        if (type.IsUserDefined)
            return $@"""{type.Name}""";
        else
            return type.Name;
    }

    public static string GetCode(this PostgreSQLDomainType type) => type.Default.Code;
    public static string GetCode(this PostgreSQLFunction func) => func.CodePiece.Code;
    public static string GetCode(this PostgreSQLProcedure proc) => proc.CodePiece.Code;

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
