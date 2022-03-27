using System;
using System.Globalization;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.MSSQL;

public static class MSSQLDefaultValueConverter
{
    public static CodePiece ConvertToMSSQL(CSharpDefaultValue defaultValue)
    {
        object value = defaultValue.CSharpValue;
        string quotedValue = value switch
        {
            long val => $"{val}",
            double val => $"{val.ToString(CultureInfo.InvariantCulture)}",
            decimal val => $"{val.ToString(CultureInfo.InvariantCulture)}",
            bool val => val ? "1" : "0",
            string val => $"'{val}'",
            byte[] val => $"{ToHexLiteral(val)}",
            DateTime val => $"'{val.ToString("yyyy-MM-dd HH:mm:ss")}'",
            Guid val => $"'{val}'",
            _ => throw new InvalidOperationException($"Invalid csharp defaultValue value: {value}"),
        };
        return new CodePiece { Code = quotedValue };

        static string ToHexLiteral(byte[] val) => $@"0x{BitConverter.ToString(val).Replace("-", "")}";
    }
}
