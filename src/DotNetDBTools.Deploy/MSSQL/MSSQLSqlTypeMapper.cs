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

        public static string GetModelType(string sqlType)
        {
            switch (sqlType.ToUpper())
            {
                case SqlNames.NVARCHAR:
                case SqlNames.VARCHAR:
                case SqlNames.NCHAR:
                case SqlNames.CHAR:
                    return DataTypeNames.String;
                case SqlNames.INT:
                    return DataTypeNames.Int;
                case SqlNames.VARBINARY:
                    return DataTypeNames.Byte;
                default:
                    return sqlType;
            }
        }

        public static string IsUnicode(string sqlType)
        {
            switch (sqlType.ToUpper())
            {
                case SqlNames.NVARCHAR:
                case SqlNames.NCHAR:
                    return true.ToString();
                case SqlNames.VARCHAR:
                case SqlNames.CHAR:
                    return false.ToString();
                default:
                    return null;
            }
        }

        public static string IsFixedLength(string sqlType)
        {
            switch (sqlType.ToUpper())
            {
                case SqlNames.NCHAR:
                case SqlNames.CHAR:
                    return true.ToString();
                case SqlNames.NVARCHAR:
                case SqlNames.VARCHAR:
                case SqlNames.VARBINARY:
                    return false.ToString();
                default:
                    return null;
            }
        }

        private static string GetStringSqlType(ColumnInfo columnInfo)
        {
            bool isUnicode = columnInfo.IsUnicode == true.ToString();
            string stringTypeName = isUnicode ? SqlNames.NVARCHAR : SqlNames.VARCHAR;

            bool isFixedLength = false;
            if (columnInfo.IsFixedLength == true.ToString())
            {
                isFixedLength = true;
                stringTypeName = isUnicode ? SqlNames.NCHAR : SqlNames.CHAR;
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
            return SqlNames.INT;
        }

        private static string GetByteSqlType(ColumnInfo columnInfo)
        {
            int length = int.Parse(columnInfo.Length);
            string lengthStr = length.ToString();
            if (length > 8000)
                lengthStr = "MAX";
            return $"{SqlNames.VARBINARY}({lengthStr})";
        }

        private static string GetUserDefinedSqlType(ColumnInfo columnInfo)
        {
            return columnInfo.DataTypeName;
        }

        private static class SqlNames
        {
            public const string NVARCHAR = nameof(NVARCHAR);
            public const string VARCHAR = nameof(VARCHAR);
            public const string NCHAR = nameof(NCHAR);
            public const string CHAR = nameof(CHAR);
            public const string INT = nameof(INT);
            public const string VARBINARY = nameof(VARBINARY);
        }
    }
}
