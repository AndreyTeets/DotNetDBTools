using System;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.SQLite
{
    internal static class SQLiteSqlTypeMapper
    {
        public static string GetSqlType(DataTypeInfo dataTypeInfo)
        {
            return dataTypeInfo.Name switch
            {
                DataTypeNames.String => GetStringSqlType(dataTypeInfo),
                DataTypeNames.Int => GetIntSqlType(dataTypeInfo),
                DataTypeNames.Binary => GetBinarySqlType(dataTypeInfo),
                _ => throw new InvalidOperationException($"Invalid dataTypeInfo.Name: '{dataTypeInfo.Name}'")
            };
        }

        public static DataTypeInfo GetModelType(string sqlType)
        {
            return sqlType.ToUpper() switch
            {
                SqlNames.TEXT => GetStringModelType(),
                SqlNames.INTEGER => GetIntModelType(),
                SqlNames.BLOB => GetBinaryModelType(),
                _ => throw new InvalidOperationException($"Invalid sqlType: '{sqlType}'")
            };
        }

        private static string GetStringSqlType(DataTypeInfo _)
        {
            return SqlNames.TEXT;
        }

        private static DataTypeInfo GetStringModelType()
        {
            return new DataTypeInfo()
            {
                Name = DataTypeNames.String,
            };
        }

        private static string GetIntSqlType(DataTypeInfo _)
        {
            return SqlNames.INTEGER;
        }

        private static DataTypeInfo GetIntModelType()
        {
            return new DataTypeInfo()
            {
                Name = DataTypeNames.Int,
            };
        }

        private static string GetBinarySqlType(DataTypeInfo _)
        {
            return SqlNames.BLOB;
        }

        private static DataTypeInfo GetBinaryModelType()
        {
            return new DataTypeInfo()
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
