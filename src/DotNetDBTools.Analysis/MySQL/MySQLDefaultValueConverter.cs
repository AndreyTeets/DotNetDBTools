﻿using System;
using System.Globalization;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.MySQL;

public static class MySQLDefaultValueConverter
{
    public static CodePiece ConvertToMySQL(CSharpDefaultValue defaultValue)
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
            Guid val => ToBinaryLiteral(val),
            TimeSpan val => $"'{val.ToString("hh':'mm':'ss")}'",
            DateTime val => $"'{val.ToString("yyyy-MM-dd")}'",
            DateTimeOffset val => FormatDateTimeOffset(val, defaultValue.IsWithTimeZone),
            _ => throw new InvalidOperationException($"Invalid csharp defaultValue value: {value}"),
        };
        return new CodePiece { Code = quotedValue };
    }

    private static string ToBinaryLiteral(byte[] val) => $@"(0x{BitConverter.ToString(val).Replace("-", "")})";
    private static string ToBinaryLiteral(Guid val) => $@"(0x{val.ToString().Replace("-", "").ToLower()})";
    private static string FormatDateTimeOffset(DateTimeOffset val, bool isWithTimeZone)
    {
        return isWithTimeZone
            ? $"'{val.ToOffset(TimeSpan.Zero).ToString("yyyy-MM-dd HH:mm:ss")}'"
            : $"'{val.ToString("yyyy-MM-dd HH:mm:ss")}'";
    }
}
