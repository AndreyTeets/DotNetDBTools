using System;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MSSQL
{
    internal static class MSSQLSqlTypeMapper
    {
        public static string GetSqlType(DataType dataType)
        {
            return dataType.Name switch
            {
                DataTypeNames.String => GetStringSqlType(dataType),
                DataTypeNames.Int => GetIntSqlType(dataType),
                DataTypeNames.Binary => GetBinarySqlType(dataType),
                DataTypeNames.DateTime => GetDateTimeSqlType(dataType),
                _ => dataType.IsUserDefined
                    ? dataType.Name
                    : throw new InvalidOperationException($"Invalid dataType.Name: '{dataType.Name}'")
            };
        }

        public static DataType GetModelType(string sqlType, string length)
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

        private static string GetStringSqlType(DataType dataType)
        {
            string stringTypeName = dataType.IsUnicode ? SqlNames.NVARCHAR : SqlNames.VARCHAR;
            if (dataType.IsFixedLength)
                stringTypeName = dataType.IsUnicode ? SqlNames.NCHAR : SqlNames.CHAR;

            string lengthStr = dataType.Length.ToString();
            if (dataType.IsUnicode && dataType.Length > 4000 ||
                !dataType.IsUnicode && dataType.Length > 8000 ||
                dataType.Length < 1)
            {
                if (dataType.IsFixedLength)
                    throw new Exception($"The size ({dataType.Length}) given to type {stringTypeName} exceeds maximum allowed length");
                lengthStr = "MAX";
            }

            return $"{stringTypeName}({lengthStr})";
        }

        private static DataType GetStringModelType(string length, bool isFixedLength, bool isUnicode)
        {
            return new DataType()
            {
                Name = DataTypeNames.String,
                Length = isUnicode ? int.Parse(length) / 2 : int.Parse(length),
                IsFixedLength = isFixedLength,
                IsUnicode = isUnicode,
            };
        }

        private static string GetIntSqlType(DataType dataType)
        {
            return dataType.Size switch
            {
                8 => SqlNames.TINYINT,
                16 => SqlNames.SMALLINT,
                32 => SqlNames.INT,
                64 => SqlNames.BIGINT,
                _ => throw new Exception($"Invalid int size: '{dataType.Size}' (this should never happen)")
            };
        }

        private static DataType GetIntModelType(int size)
        {
            return new DataType()
            {
                Name = DataTypeNames.Int,
                Size = size,
            };
        }

        private static string GetBinarySqlType(DataType dataType)
        {
            string binaryTypeName = dataType.IsFixedLength ? SqlNames.BINARY : SqlNames.VARBINARY;

            string lengthStr = dataType.Length.ToString();
            if (dataType.Length > 8000 ||
                dataType.Length < 1)
            {
                if (dataType.IsFixedLength)
                    throw new Exception($"The size ({dataType.Length}) given to type {binaryTypeName} exceeds maximum allowed length");
                lengthStr = "MAX";
            }

            return $"{binaryTypeName}({lengthStr})";
        }

        private static DataType GetBinaryModelType(string length, bool isFixedLength)
        {
            return new DataType()
            {
                Name = DataTypeNames.Binary,
                Length = int.Parse(length),
                IsFixedLength = isFixedLength,
            };
        }

        private static string GetDateTimeSqlType(DataType dataType)
        {
            return dataType.SqlTypeName;
        }

        private static DataType GetDateTimeModelType(string sqlType)
        {
            return new DataType()
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
