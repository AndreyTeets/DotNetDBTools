using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Deploy.PostgreSQL
{
    internal static class PostgreSQLDbObjectsExtensions
    {
        public static string GetQuotedName(this DataType type)
        {
            if (type.IsUserDefined)
                return $@"""{type.Name}""";
            else
                return type.Name;
        }

        public static string GetCode(this PostgreSQLDomainType type) => (type.Default as CodePiece)?.Code;
        public static string GetCode(this PostgreSQLFunction func) => func.CodePiece.Code;
        public static string GetCode(this PostgreSQLProcedure proc) => proc.CodePiece.Code;
    }
}
