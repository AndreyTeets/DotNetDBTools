using System;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Deploy.PostgreSQL
{
    internal static class PostgreSQLQueriesHelper
    {
        public static string GetIdentityStatement(Column column)
        {
            return column.Identity ? " IDENTITY" : "";
        }

        public static string GetNullabilityStatement(Column column) =>
            column.Nullable switch
            {
                true => "NULL",
                false => "NOT NULL",
            };

        public static string QuoteDefaultValue(object value)
        {
            return value switch
            {
                PostgreSQLDefaultValueAsFunction => $"{((PostgreSQLDefaultValueAsFunction)value).FunctionText}",
                string => $"'{value}'",
                long => $"{value}",
                byte[] => $"{ToHex((byte[])value)}",
                _ => throw new InvalidOperationException($"Invalid value type: '{value.GetType()}'")
            };

            static string ToHex(byte[] val) => "0x" + BitConverter.ToString(val).Replace("-", "");
        }

        public static string MapActionName(string modelActionName) =>
            modelActionName switch
            {
                "NoAction" => "NO ACTION",
                "Cascade" => "CASCADE",
                "SetDefault" => "SET DEFAULT",
                "SetNull" => "SET NULL",
                _ => throw new InvalidOperationException($"Invalid modelActionName: '{modelActionName}'")
            };

        public static DataType CreateDataTypeModel(string dataType, int length)
        {
            switch (dataType)
            {
                case PostgreSQLDataTypeNames.SMALLINT:
                case PostgreSQLDataTypeNames.INT:
                case PostgreSQLDataTypeNames.BIGINT:

                case PostgreSQLDataTypeNames.REAL:
                case PostgreSQLDataTypeNames.FLOAT8:
                case PostgreSQLDataTypeNames.DECIMAL:
                case PostgreSQLDataTypeNames.BOOL:

                case PostgreSQLDataTypeNames.MONEY:

                case PostgreSQLDataTypeNames.TEXT:
                case PostgreSQLDataTypeNames.BYTEA:

                case PostgreSQLDataTypeNames.DATE:
                case PostgreSQLDataTypeNames.TIME:
                case PostgreSQLDataTypeNames.TIMETZ:
                case PostgreSQLDataTypeNames.TIMESTAMP:
                case PostgreSQLDataTypeNames.TIMESTAMPTZ:

                case PostgreSQLDataTypeNames.UUID:
                case PostgreSQLDataTypeNames.JSON:
                case PostgreSQLDataTypeNames.JSONB:
                    return new DataType { Name = dataType };

                case PostgreSQLDataTypeNames.CHAR:
                case PostgreSQLDataTypeNames.VARCHAR:

                case PostgreSQLDataTypeNames.BIT:
                case PostgreSQLDataTypeNames.VARBIT:
                    return new DataType { Name = $"{dataType}({length})" };

                default:
                    throw new InvalidOperationException($"Invalid datatype: {dataType}");
            }
        }
    }
}
