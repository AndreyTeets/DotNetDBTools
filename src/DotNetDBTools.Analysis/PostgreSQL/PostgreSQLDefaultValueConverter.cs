using System;
using System.Globalization;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.PostgreSQL;

public static class PostgreSQLDefaultValueConverter
{
    public static CodePiece ConvertToPostgreSQL(CSharpDefaultValue defaultValue)
    {
        object value = defaultValue.CSharpValue;
        string quotedValue = value switch
        {
            long val => $"{val}",
            double val => $"{val.ToString(CultureInfo.InvariantCulture)}",
            decimal val => $"{val.ToString(CultureInfo.InvariantCulture)}",
            bool val => val ? "TRUE" : "FALSE",
            string val => $"'{val}'",
            byte[] val => $"{ToHexLiteral(val)}",
            DateTime val => $"'{val.ToString("yyyy-MM-dd HH:mm:ss")}'",
            Guid val => $"'{val}'",
            _ => throw new InvalidOperationException($"Invalid csharp defaultValue value: {value}"),
        };
        return new CodePiece { Code = quotedValue };

        static string ToHexLiteral(byte[] val) => $@"'\x{BitConverter.ToString(val).Replace("-", "")}'";
    }
}
