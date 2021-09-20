using System;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MSSQL
{
    public static class MSSQLSqlTypeMapper
    {
        public static string GetSqlType(ColumnInfo columnInfo)
        {
            return columnInfo.DataTypeName switch
            {
                DataTypeNames.String => GetStringSqlType(columnInfo),
                DataTypeNames.Int => GetIntSqlType(columnInfo),
                DataTypeNames.Byte => GetByteSqlType(columnInfo),
                _ => GetUserDefinedSqlType(columnInfo)
            };
        }

        private static string GetStringSqlType(ColumnInfo columnInfo)
        {
            bool isUnicode = columnInfo.IsUnicode == true.ToString();
            string stringTypeName = isUnicode ? "NVARCHAR" : "VARCHAR";

            bool isFixedLength = false;
            if (columnInfo.IsFixedLength == true.ToString())
            {
                isFixedLength = true;
                stringTypeName = isUnicode ? "NCHAR" : "CHAR";
            }

            int length = int.Parse(columnInfo.Length);
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

        private static string GetIntSqlType(ColumnInfo _)
        {
            return "INT";
        }

        private static string GetByteSqlType(ColumnInfo columnInfo)
        {
            int length = int.Parse(columnInfo.Length);
            string lengthStr = length.ToString();
            if (length > 8000)
                lengthStr = "MAX";
            return $"VARBINARY({lengthStr})";
        }

        private static string GetUserDefinedSqlType(ColumnInfo columnInfo)
        {
            return columnInfo.DataTypeName;
        }
    }
}
