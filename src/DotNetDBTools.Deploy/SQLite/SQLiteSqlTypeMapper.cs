using System;
using DotNetDBTools.Models.Shared;

namespace DotNetDBTools.Deploy.SQLite
{
    public static class SQLiteSqlTypeMapper
    {
        public static string GetSqlType(DataTypeInfo dataTypeInfo)
        {
            return dataTypeInfo.Name switch
            {
                DataTypeNames.String => GetStringSqlType(dataTypeInfo),
                DataTypeNames.Int => GetIntSqlType(dataTypeInfo),
                DataTypeNames.Byte => GetByteSqlType(dataTypeInfo),
                _ => throw new InvalidOperationException($"Invalid data type name '{dataTypeInfo.Name}'")
            };
        }

        private static string GetStringSqlType(DataTypeInfo _)
        {
            return "TEXT";
        }

        private static string GetIntSqlType(DataTypeInfo _)
        {
            return "INTEGER";
        }

        private static string GetByteSqlType(DataTypeInfo _)
        {
            return "BLOB";
        }
    }
}
