using System;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Deploy.PostgreSQL
{
    internal static class PostgreSQLQueriesHelper
    {
        public static string GetIdentityStatement(Column column)
        {
            return column.Identity ? " GENERATED ALWAYS AS IDENTITY" : "";
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

            static string ToHex(byte[] val) => $@"'\x{BitConverter.ToString(val).Replace("-", "")}'";
        }

        public static string MapActionName(string modelActionName) =>
            modelActionName switch
            {
                "NoAction" => "NO ACTION",
                "Restrict" => "RESTRICT",
                "Cascade" => "CASCADE",
                "SetDefault" => "SET DEFAULT",
                "SetNull" => "SET NULL",
                _ => throw new InvalidOperationException($"Invalid modelActionName: '{modelActionName}'")
            };

        public static DataType CreateDataTypeModel(string dataType, int length)
        {
            string normalizedDataType = NormalizeTypeName(dataType);
            switch (normalizedDataType)
            {
                case PostgreSQLDataTypeNames.SMALLINT:
                case PostgreSQLDataTypeNames.INT:
                case PostgreSQLDataTypeNames.BIGINT:

                case PostgreSQLDataTypeNames.FLOAT4:
                case PostgreSQLDataTypeNames.FLOAT8:
                case PostgreSQLDataTypeNames.BOOL:

                case PostgreSQLDataTypeNames.MONEY:

                case PostgreSQLDataTypeNames.TEXT:
                case PostgreSQLDataTypeNames.BYTEA:

                case PostgreSQLDataTypeNames.DATE:

                case PostgreSQLDataTypeNames.UUID:
                case PostgreSQLDataTypeNames.JSON:
                case PostgreSQLDataTypeNames.JSONB:
                    return new DataType { Name = normalizedDataType };

                case PostgreSQLDataTypeNames.DECIMAL:
                    return new DataType { Name = $"{normalizedDataType}({GetDecimalPrecisionAndScale(length)})" };

                case PostgreSQLDataTypeNames.CHAR:
                case PostgreSQLDataTypeNames.VARCHAR:
                    return new DataType { Name = $"{normalizedDataType}({length - 4})" };

                case PostgreSQLDataTypeNames.TIME:
                case PostgreSQLDataTypeNames.TIMETZ:
                case PostgreSQLDataTypeNames.TIMESTAMP:
                case PostgreSQLDataTypeNames.TIMESTAMPTZ:

                case PostgreSQLDataTypeNames.BIT:
                case PostgreSQLDataTypeNames.VARBIT:
                    return new DataType { Name = $"{normalizedDataType}({length})" };

                default:
                    throw new InvalidOperationException($"Invalid normalized datatype: {normalizedDataType}");
            }

            string NormalizeTypeName(string dataType)
            {
                return dataType.ToUpper() switch
                {
                    "INT2" => PostgreSQLDataTypeNames.SMALLINT,
                    "INT4" => PostgreSQLDataTypeNames.INT,
                    "INT8" => PostgreSQLDataTypeNames.BIGINT,
                    "NUMERIC" => PostgreSQLDataTypeNames.DECIMAL,
                    "BPCHAR" => PostgreSQLDataTypeNames.CHAR,
                    _ => dataType.ToUpper(),
                };
            }

            string GetDecimalPrecisionAndScale(int length)
            {
                int precision = ((length - 4) >> 16) & 65535;
                int scale = (length - 4) & 65535;
                return $"{precision}, {scale}";
            }
        }
    }
}
