using System;
using DotNetDBTools.Definition;
using DotNetDBTools.Definition.BaseDbTypes;

namespace DotNetDBTools.DefinitionParser.SQLite
{
    public static class ColumnTypeMapper
    {
        public static string GetSqlType(IDbType dbType)
        {
            return dbType switch
            {
                BaseStringDbType baseStringDbType => GetStringSqlType(baseStringDbType),
                BaseIntDbType baseIntDbType => GetIntSqlType(baseIntDbType),
                BaseByteDbType baseByteDbType => GetByteSqlType(baseByteDbType),
                _ => throw new InvalidOperationException($"Invalid dbType: {dbType}")
            };
        }

        private static string GetStringSqlType(BaseStringDbType _)
        {
            return "TEXT";
        }

        private static string GetIntSqlType(BaseIntDbType _)
        {
            return "INTEGER";
        }

        private static string GetByteSqlType(BaseByteDbType _)
        {
            return "BLOB";
        }
    }
}
