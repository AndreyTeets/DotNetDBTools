using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL;

internal static class MSSQLHelperExtensions
{
    public static string GetCode(this MSSQLFunction func) => func.CreateStatement.Code;
    public static string GetCode(this MSSQLProcedure proc) => proc.CreateStatement.Code;
}
