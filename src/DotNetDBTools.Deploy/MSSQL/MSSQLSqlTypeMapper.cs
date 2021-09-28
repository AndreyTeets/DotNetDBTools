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
                DataTypeNames.Binary => GetBinarySqlType(dataTypeInfo),
                DataTypeNames.DateTime => GetDateTimeSqlType(dataTypeInfo),
                _ => dataTypeInfo.IsUserDefined
                    ? dataTypeInfo.Name
                    : throw new InvalidOperationException($"Invalid dataTypeInfo.Name: '{dataTypeInfo.Name}'")
            };
        }

        public static DataTypeInfo GetModelType(string sqlType, string length)
        {
            return sqlType.ToUpper() switch
            {
                SqlNames.NVARCHAR => GetStringModelType(length, isFixedLength: false, isUnicode: true),
                SqlNames.VARCHAR => GetStringModelType(length, isFixedLength: false, isUnicode: false),
                SqlNames.NCHAR => GetStringModelType(length, isFixedLength: true, isUnicode: true),
                SqlNames.CHAR => GetStringModelType(length, isFixedLength: true, isUnicode: false),
                SqlNames.TINYINT => GetIntModelType(8),
                SqlNames.SMALLINT => GetIntModelType(16),
                SqlNames.INT => GetIntModelType(32),
                SqlNames.BIGINT => GetIntModelType(64),
                SqlNames.VARBINARY => GetBinaryModelType(length, isFixedLength: false),
                SqlNames.BINARY => GetBinaryModelType(length, isFixedLength: true),
                SqlNames.SMALLDATETIME => GetDateTimeModelType(sqlType),
                SqlNames.DATETIME => GetDateTimeModelType(sqlType),
                SqlNames.DATETIME2 => GetDateTimeModelType(sqlType),
                SqlNames.DATETIMEOFFSET => GetDateTimeModelType(sqlType),
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
                dataTypeInfo.Length < 1)
            {
                if (dataTypeInfo.IsFixedLength)
                    throw new Exception($"The size ({dataTypeInfo.Length}) given to type {stringTypeName} exceeds maximum allowed length");
                lengthStr = "MAX";
            }

            return $"{stringTypeName}({lengthStr})";
        }

        private static DataTypeInfo GetStringModelType(string length, bool isFixedLength, bool isUnicode)
        {
            return new DataTypeInfo()
            {
                Name = DataTypeNames.String,
                Length = isUnicode ? int.Parse(length) / 2 : int.Parse(length),
                IsFixedLength = isFixedLength,
                IsUnicode = isUnicode,
            };
        }

        private static string GetIntSqlType(DataTypeInfo dataTypeInfo)
        {
            return dataTypeInfo.Size switch
            {
                8 => SqlNames.TINYINT,
                16 => SqlNames.SMALLINT,
                32 => SqlNames.INT,
                64 => SqlNames.BIGINT,
                _ => throw new Exception($"Invalid int size: '{dataTypeInfo.Size}' (this should never happen)")
            };
        }

        private static DataTypeInfo GetIntModelType(int size)
        {
            return new DataTypeInfo()
            {
                Name = DataTypeNames.Int,
                Size = size,
            };
        }

        private static string GetBinarySqlType(DataTypeInfo dataTypeInfo)
        {
            string binaryTypeName = dataTypeInfo.IsFixedLength ? SqlNames.BINARY : SqlNames.VARBINARY;

            string lengthStr = dataTypeInfo.Length.ToString();
            if (dataTypeInfo.Length > 8000 ||
                dataTypeInfo.Length < 1)
            {
                if (dataTypeInfo.IsFixedLength)
                    throw new Exception($"The size ({dataTypeInfo.Length}) given to type {binaryTypeName} exceeds maximum allowed length");
                lengthStr = "MAX";
            }

            return $"{binaryTypeName}({lengthStr})";
        }

        private static DataTypeInfo GetBinaryModelType(string length, bool isFixedLength)
        {
            return new DataTypeInfo()
            {
                Name = DataTypeNames.Binary,
                Length = int.Parse(length),
                IsFixedLength = isFixedLength,
            };
        }

        private static string GetDateTimeSqlType(DataTypeInfo dataTypeInfo)
        {
            return dataTypeInfo.SqlTypeName;
        }

        private static DataTypeInfo GetDateTimeModelType(string sqlType)
        {
            return new DataTypeInfo()
            {
                Name = DataTypeNames.DateTime,
                SqlTypeName = sqlType.ToUpper(),
            };
        }

        private static class SqlNames
        {
            public const string NVARCHAR = nameof(NVARCHAR);
            public const string VARCHAR = nameof(VARCHAR);
            public const string NCHAR = nameof(NCHAR);
            public const string CHAR = nameof(CHAR);
            public const string TINYINT = nameof(TINYINT);
            public const string SMALLINT = nameof(SMALLINT);
            public const string INT = nameof(INT);
            public const string BIGINT = nameof(BIGINT);
            public const string VARBINARY = nameof(VARBINARY);
            public const string BINARY = nameof(BINARY);
            public const string SMALLDATETIME = nameof(SMALLDATETIME);
            public const string DATETIME = nameof(DATETIME);
            public const string DATETIME2 = nameof(DATETIME2);
            public const string DATETIMEOFFSET = nameof(DATETIMEOFFSET);
        }
    }
}
