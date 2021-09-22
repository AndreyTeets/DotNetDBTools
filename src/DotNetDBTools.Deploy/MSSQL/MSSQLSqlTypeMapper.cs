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
                _ => dataTypeInfo.IsUserDefined
                    ? dataTypeInfo.Name
                    : throw new InvalidOperationException($"Invalid dataTypeInfo.Name: '{dataTypeInfo.Name}'")
            };
        }

        public static DataTypeInfo GetModelType(string sqlType, string length)
        {
            return sqlType.ToUpper() switch
            {
                SqlNames.NVARCHAR => GetStringModelType(length, isUnicode: true, isFixedLength: false),
                SqlNames.VARCHAR => GetStringModelType(length, isUnicode: false, isFixedLength: false),
                SqlNames.NCHAR => GetStringModelType(length, isUnicode: true, isFixedLength: true),
                SqlNames.CHAR => GetStringModelType(length, isUnicode: false, isFixedLength: true),
                SqlNames.INT => GetIntModelType(),
                SqlNames.VARBINARY => GetByteModelType(length),
                _ => throw new InvalidOperationException($"Invalid sqlType: '{sqlType}'")
            };
        }

        private static string GetStringSqlType(DataTypeInfo dataTypeInfo)
        {
            string stringTypeName = dataTypeInfo.IsUnicode ? SqlNames.NVARCHAR : SqlNames.VARCHAR;
            if (dataTypeInfo.IsFixedLength)
                stringTypeName = dataTypeInfo.IsUnicode ? SqlNames.NCHAR : SqlNames.CHAR;

            string lengthStr = dataTypeInfo.Length.ToString();
            if (dataTypeInfo.IsUnicode && dataTypeInfo.Length > 4000 ||
                !dataTypeInfo.IsUnicode && dataTypeInfo.Length > 8000 ||
                dataTypeInfo.Length == -1)
            {
                if (dataTypeInfo.IsFixedLength)
                    throw new Exception($"The size ({dataTypeInfo.Length}) given to type {stringTypeName} exceeds maximum allowed length");
                lengthStr = "MAX";
            }

            return $"{stringTypeName}({lengthStr})";
        }

        private static DataTypeInfo GetStringModelType(string length, bool isUnicode, bool isFixedLength)
        {
            return new DataTypeInfo()
            {
                Name = DataTypeNames.String,
                Length = int.Parse(length),
                IsUnicode = isUnicode,
                IsFixedLength = isFixedLength,
            };
        }

        private static string GetIntSqlType(DataTypeInfo _)
        {
            return SqlNames.INT;
        }

        private static DataTypeInfo GetIntModelType()
        {
            return new DataTypeInfo()
            {
                Name = DataTypeNames.Int,
            };
        }

        private static string GetByteSqlType(DataTypeInfo dataTypeInfo)
        {
            string lengthStr = dataTypeInfo.Length.ToString();
            if (dataTypeInfo.Length > 8000 || dataTypeInfo.Length == -1)
                lengthStr = "MAX";
            return $"{SqlNames.VARBINARY}({lengthStr})";
        }

        private static DataTypeInfo GetByteModelType(string length)
        {
            return new DataTypeInfo()
            {
                Name = DataTypeNames.Byte,
                Length = int.Parse(length),
            };
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
