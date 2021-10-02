using System;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.SQLite
{
    internal static class SQLiteSqlTypeMapper
    {
        public static string GetSqlType(DataType dataType)
        {
            return dataType.Name switch
            {
                DataTypeNames.String => GetStringSqlType(dataType),
                DataTypeNames.Int => GetIntSqlType(dataType),
                DataTypeNames.Binary => GetBinarySqlType(dataType),
                _ => throw new InvalidOperationException($"Invalid dataTypeInfo.Name: '{dataType.Name}'")
            };
        }

        public static DataType GetModelType(string sqlType)
        {
            return sqlType.ToUpper() switch
            {
                SqlNames.TEXT => GetStringModelType(),
                SqlNames.INTEGER => GetIntModelType(),
                SqlNames.BLOB => GetBinaryModelType(),
                _ => throw new InvalidOperationException($"Invalid sqlType: '{sqlType}'")
            };
        }

        private static string GetStringSqlType(DataType _)
        {
            return SqlNames.TEXT;
        }

        private static DataType GetStringModelType()
        {
            return new DataType()
            {
                Name = DataTypeNames.String,
            };
        }

        private static string GetIntSqlType(DataType _)
        {
            return SqlNames.INTEGER;
        }

        private static DataType GetIntModelType()
        {
            return new DataType()
            {
                Name = DataTypeNames.Int,
            };
        }

        private static string GetBinarySqlType(DataType _)
        {
            return SqlNames.BLOB;
        }

        private static DataType GetBinaryModelType()
        {
            return new DataType()
            {
                Name = DataTypeNames.Binary,
            };
        }

        private static class SqlNames
        {
            public const string TEXT = nameof(TEXT);
            public const string INTEGER = nameof(INTEGER);
            public const string BLOB = nameof(BLOB);
        }
    }
}
