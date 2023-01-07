using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Generation.MSSQL;

public static class MSSQLHelperExtensions
{
    public static string GetCreateStatement(this MSSQLFunction func) => func.CreateStatement.Code;
    public static string GetCreateStatement(this MSSQLProcedure proc) => proc.CreateStatement.Code;
}
