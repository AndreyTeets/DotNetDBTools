using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Generation.MSSQL;

public static class MSSQLHelperExtensions
{
    public static string GetCode(this MSSQLFunction func) => func.CodePiece.Code;
    public static string GetCode(this MSSQLProcedure proc) => proc.CodePiece.Code;
}
