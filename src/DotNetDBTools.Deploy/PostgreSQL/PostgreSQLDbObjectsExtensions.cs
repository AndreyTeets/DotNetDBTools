using DotNetDBTools.Models.Core;
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

        public static string GetExtraInfo(this PostgreSQLDomainType type)
        {
            return (type.Default as DefaultValueAsFunction)?.FunctionText;
        }
    }
}
