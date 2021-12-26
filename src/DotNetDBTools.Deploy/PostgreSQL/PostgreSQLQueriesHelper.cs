using System;
using System.Linq;
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
                CodePiece => $"{((CodePiece)value).Code}",
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

        public static DataType CreateDataTypeModel(string dataType, string lengthStr, bool isBaseDataType)
        {
            if (!isBaseDataType)
                return new DataType { Name = dataType, IsUserDefined = true };

            // TODO handle user defined base data types

            int length = int.Parse(lengthStr);
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
                    return new DataType { Name = length == -1 ? normalizedDataType : $"{normalizedDataType}({GetDecimalPrecisionAndScale(length)})" };

                case PostgreSQLDataTypeNames.CHAR:
                case PostgreSQLDataTypeNames.VARCHAR:
                    return new DataType { Name = length == -1 ? normalizedDataType : $"{normalizedDataType}({length - 4})" };

                case PostgreSQLDataTypeNames.TIME:
                case PostgreSQLDataTypeNames.TIMETZ:
                case PostgreSQLDataTypeNames.TIMESTAMP:
                case PostgreSQLDataTypeNames.TIMESTAMPTZ:
                    return new DataType { Name = length == -1 ? normalizedDataType : $"{normalizedDataType}({length})" };

                case PostgreSQLDataTypeNames.BIT:
                case PostgreSQLDataTypeNames.VARBIT:
                    return new DataType { Name = length == -1 ? normalizedDataType : $"{normalizedDataType}({length})" };

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

        public static object ParseDefault(string valueFromDBMSSysTable)
        {
            if (valueFromDBMSSysTable is null)
                return null;
            string value = valueFromDBMSSysTable;

            if (IsFunction(value))
                return new CodePiece() { Code = value };
            if (IsByte(value))
                return ToByteArray(value);
            if (IsString(value))
                return TrimOuterQuotes(value.Remove(value.LastIndexOf("::", StringComparison.Ordinal)));
            if (IsNumber(value))
                return long.Parse(value);

            throw new ArgumentException($"Invalid parameter value '{valueFromDBMSSysTable}'", nameof(valueFromDBMSSysTable));

            static bool IsFunction(string val) => val.Contains("(") && val.Contains(")");
            static bool IsString(string val) => !IsFunction(val) && new string[] { "::text", "::character" }.Any(x => val.Contains(x));
            static bool IsByte(string val) => !IsFunction(val) && val.Contains("::bytea");
            static bool IsNumber(string val) => !IsFunction(val) && long.TryParse(val, out _);
            static string TrimOuterQuotes(string val) => val.Substring(1, val.Length - 2);
            static byte[] ToByteArray(string val)
            {
                string hex = TrimOuterQuotes(val.Substring(2, val.Length - 2).Replace("::bytea", ""));
                int numChars = hex.Length;
                byte[] bytes = new byte[numChars / 2];
                for (int i = 0; i < numChars; i += 2)
                    bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
                return bytes;
            }
        }
    }
}
