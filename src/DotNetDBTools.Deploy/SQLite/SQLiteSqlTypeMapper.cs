using System;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.SQLite
{
    public static class SQLiteSqlTypeMapper
    {
        public static string GetSqlType(ColumnInfo columnInfo)
        {
            return columnInfo.DataTypeName switch
            {
                DataTypeNames.String => GetStringSqlType(columnInfo),
                DataTypeNames.Int => GetIntSqlType(columnInfo),
                DataTypeNames.Byte => GetByteSqlType(columnInfo),
                _ => throw new InvalidOperationException($"Invalid data type name '{columnInfo.DataTypeName}'")
            };
        }

        private static string GetStringSqlType(ColumnInfo _)
        {
            return "TEXT";
        }

        private static string GetIntSqlType(ColumnInfo _)
        {
            return "INTEGER";
        }

        private static string GetByteSqlType(ColumnInfo _)
        {
            return "BLOB";
        }
    }
}
