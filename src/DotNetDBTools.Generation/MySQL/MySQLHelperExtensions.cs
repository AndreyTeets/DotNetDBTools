using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Generation.MySQL;

public static class MySQLHelperExtensions
{
    public static string GetCreateStatement(this MySQLFunction func) => func.CreateStatement.Code;
    public static string GetCreateStatement(this MySQLProcedure proc) => proc.CreateStatement.Code;
}
