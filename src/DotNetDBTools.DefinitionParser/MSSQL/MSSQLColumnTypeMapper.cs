using System;
using DotNetDBTools.Definition;
using DotNetDBTools.Definition.BaseDbTypes;
using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.Definition.MSSQL.DbTypes;

namespace DotNetDBTools.DefinitionParser.MSSQL
{
    public static class MSSQLColumnTypeMapper
    {
        public static string GetSqlType(IDbType dbType)
        {
            return dbType switch
            {
                BaseStringDbType baseStringDbType => GetStringSqlType(baseStringDbType),
                BaseIntDbType baseIntDbType => GetIntSqlType(baseIntDbType),
                BaseByteDbType baseByteDbType => GetByteSqlType(baseByteDbType),
                IUserDefinedType userDefinedType => GetUserDefinedSqlType(userDefinedType),
                _ => throw new InvalidOperationException($"Invalid dbType: {dbType}")
            };
        }

        private static string GetStringSqlType(BaseStringDbType baseStringDbType)
        {
            string stringTypeName = baseStringDbType.IsUnicode ? "NVARCHAR" : "VARCHAR";
            bool isFixedLength = false;
            if (baseStringDbType is StringDbType stringDbType &&
                stringDbType.IsFixedLength)
            {
                isFixedLength = true;
                stringTypeName = stringDbType.IsUnicode ? "NCHAR" : "CHAR";
            }

            string length = baseStringDbType.Length.ToString();
            if (baseStringDbType.IsUnicode && baseStringDbType.Length > 4000 ||
                !baseStringDbType.IsUnicode && baseStringDbType.Length > 8000)
            {
                if (isFixedLength)
                    throw new Exception($"The size ({baseStringDbType.Length}) given to type {stringTypeName} exceeds maximum allowed length");
                length = "MAX";
            }

            return $"{stringTypeName}({length})";
        }

        private static string GetIntSqlType(BaseIntDbType _)
        {
            return "INT";
        }

        private static string GetByteSqlType(BaseByteDbType baseByteDbType)
        {
            string length = baseByteDbType.Length.ToString();
            if (baseByteDbType.Length > 8000)
                length = "MAX";
            return $"VARBINARY({length})";
        }

        private static string GetUserDefinedSqlType(IUserDefinedType userDefinedType)
        {
            string userDefinedTypeName = userDefinedType.GetType().Name;
            return userDefinedTypeName;
        }
    }
}
