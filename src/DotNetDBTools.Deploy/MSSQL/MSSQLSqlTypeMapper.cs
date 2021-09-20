using System;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MSSQL
{
    public static class MSSQLSqlTypeMapper
    {
        public static string GetSqlType(DataTypeInfo dataTypeInfo)
        {
            return dataTypeInfo.Name switch
            {
                DataTypeNames.String => GetStringSqlType(dataTypeInfo),
                DataTypeNames.Int => GetIntSqlType(dataTypeInfo),
                DataTypeNames.Byte => GetByteSqlType(dataTypeInfo),
                _ => GetUserDefinedSqlType(dataTypeInfo)
            };
        }

        private static string GetStringSqlType(DataTypeInfo dataTypeInfo)
        {
            bool isUnicode = dataTypeInfo.Attributes[DataTypeAttributes.IsUnicode] == true.ToString();
            string stringTypeName = isUnicode ? "NVARCHAR" : "VARCHAR";

            bool isFixedLength = false;
            if (dataTypeInfo.Attributes[DataTypeAttributes.IsFixedLength] == true.ToString())
            {
                isFixedLength = true;
                stringTypeName = isUnicode ? "NCHAR" : "CHAR";
            }

            int length = int.Parse(dataTypeInfo.Attributes[DataTypeAttributes.Length]);
            string lengthStr = length.ToString();
            if (isUnicode && length > 4000 ||
                !isUnicode && length > 8000)
            {
                if (isFixedLength)
                    throw new Exception($"The size ({length}) given to type {stringTypeName} exceeds maximum allowed length");
                lengthStr = "MAX";
            }

            return $"{stringTypeName}({lengthStr})";
        }

        private static string GetIntSqlType(DataTypeInfo _)
        {
            return "INT";
        }

        private static string GetByteSqlType(DataTypeInfo dataTypeInfo)
        {
            int length = int.Parse(dataTypeInfo.Attributes[DataTypeAttributes.Length]);
            string lengthStr = length.ToString();
            if (length > 8000)
                lengthStr = "MAX";
            return $"VARBINARY({lengthStr})";
        }

        private static string GetUserDefinedSqlType(DataTypeInfo dataTypeInfo)
        {
            return dataTypeInfo.Name;
        }
    }
}
