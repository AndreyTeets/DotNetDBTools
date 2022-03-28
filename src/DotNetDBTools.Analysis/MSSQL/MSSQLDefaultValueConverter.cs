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
            long val => val.ToString(),
            double val => val.ToString(CultureInfo.InvariantCulture),
            decimal val => val.ToString(CultureInfo.InvariantCulture),
            bool val => val ? "1" : "0",
            string val => $"'{val}'",
            byte[] val => ToBinaryLiteral(val),
            Guid val => $"'{val}'",
            TimeSpan val => $"'{val.ToString("hh':'mm':'ss")}'",
            DateTime val => $"'{val.ToString("yyyy-MM-dd")}'",
            DateTimeOffset val => FormatDateTimeOffset(val, defaultValue.IsWithTimeZone),
            _ => throw new InvalidOperationException($"Invalid csharp defaultValue value: {value}"),
        };
        return new CodePiece { Code = quotedValue };
    }

    private static string ToBinaryLiteral(byte[] val) => $@"0x{BitConverter.ToString(val).Replace("-", "")}";
    private static string FormatDateTimeOffset(DateTimeOffset val, bool isWithTimeZone)
    {
        return isWithTimeZone
            ? $"'{val.ToString("yyyy-MM-dd HH:mm:sszzz")}'"
            : $"'{val.ToString("yyyy-MM-dd HH:mm:ss")}'";
    }
}
