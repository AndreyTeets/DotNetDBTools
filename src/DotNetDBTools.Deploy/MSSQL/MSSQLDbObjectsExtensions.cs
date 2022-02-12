using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL;

internal static class MSSQLDbObjectsExtensions
{
    public static string GetCode(this MSSQLFunction func) => func.CodePiece.Code;
    public static string GetCode(this MSSQLProcedure proc) => proc.CodePiece.Code;
}
