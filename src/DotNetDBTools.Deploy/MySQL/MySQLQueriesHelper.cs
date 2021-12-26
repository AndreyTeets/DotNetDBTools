using System;
using System.Globalization;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Deploy.MySQL
{
    internal static class MySQLQueriesHelper
    {
        public static string GetIdentityStatement(Column column)
        {
            return column.Identity ? " AUTO_INCREMENT" : "";
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
                CodePiece => $"({((CodePiece)value).Code})",
                string => $"('{value}')",
                long => $"({value})",
                decimal val => $"{val.ToString(CultureInfo.InvariantCulture)}",
                byte[] => $"({ToHex((byte[])value)})",
                _ => throw new InvalidOperationException($"Invalid value type: '{value.GetType()}'")
            };

            static string ToHex(byte[] val) => $@"0x{BitConverter.ToString(val).Replace("-", "")}";
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

        public static DataType CreateDataTypeModel(string dataType, string fullDataType)
        {
            switch (dataType)
            {
                case MySQLDataTypeNames.TINYINT:
                case MySQLDataTypeNames.SMALLINT:
                case MySQLDataTypeNames.MEDIUMINT:
                case MySQLDataTypeNames.INT:
                case MySQLDataTypeNames.BIGINT:

                case MySQLDataTypeNames.FLOAT:
                case MySQLDataTypeNames.DOUBLE:
                case MySQLDataTypeNames.DECIMAL:

                case MySQLDataTypeNames.TINYTEXT:
                case MySQLDataTypeNames.TEXT:
                case MySQLDataTypeNames.MEDIUMTEXT:
                case MySQLDataTypeNames.LONGTEXT:

                case MySQLDataTypeNames.TINYBLOB:
                case MySQLDataTypeNames.BLOB:
                case MySQLDataTypeNames.MEDIUMBLOB:
                case MySQLDataTypeNames.LONGBLOB:

                case MySQLDataTypeNames.DATE:
                case MySQLDataTypeNames.TIME:
                case MySQLDataTypeNames.SMALLDATETIME:
                case MySQLDataTypeNames.DATETIME:
                case MySQLDataTypeNames.TIMESTAMP:
                case MySQLDataTypeNames.YEAR:

                case MySQLDataTypeNames.JSON:
                case MySQLDataTypeNames.ENUM:
                case MySQLDataTypeNames.SET:

                case MySQLDataTypeNames.CHAR:
                case MySQLDataTypeNames.VARCHAR:

                case MySQLDataTypeNames.BINARY:
                case MySQLDataTypeNames.VARBINARY:

                case MySQLDataTypeNames.BIT:
                    return new DataType { Name = fullDataType };

                default:
                    throw new InvalidOperationException($"Invalid datatype: {dataType}");
            }
        }
    }
}
