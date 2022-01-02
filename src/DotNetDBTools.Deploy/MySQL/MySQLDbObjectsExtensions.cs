using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Deploy.MySQL
{
    internal static class MySQLDbObjectsExtensions
    {
        public static string GetCode(this MySQLFunction func) => func.CodePiece.Code;
        public static string GetCode(this MySQLProcedure proc) => proc.CodePiece.Code;
    }
}
