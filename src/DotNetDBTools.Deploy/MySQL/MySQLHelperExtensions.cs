using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Deploy.MySQL;

internal static class MySQLHelperExtensions
{
    public static string GetCode(this MySQLFunction func) => func.CreateStatement.Code;
    public static string GetCode(this MySQLProcedure proc) => proc.CreateStatement.Code;
}
