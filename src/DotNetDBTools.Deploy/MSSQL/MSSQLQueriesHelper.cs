using System;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL
{
    internal static class MSSQLQueriesHelper
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
                MSSQLDefaultValueAsFunction => $"{((MSSQLDefaultValueAsFunction)value).FunctionText}",
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
    }
}
