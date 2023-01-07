using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Analysis.PostgreSQL;

internal static class PostgreSQLHelperMethods
{
    private const string P = @"(?<precision>\([\s\d\,\-]+\))?";
    private const string A = @"(?<array>(:?\[[\s\d]*\]\s*)*)";

    public static HashSet<string> GetUserDefinedTypesName(Database database)
    {
        PostgreSQLDatabase db = (PostgreSQLDatabase)database;
        IEnumerable<DbObject> userDefinedTypes =
            db.CompositeTypes.Select(x => (DbObject)x)
            .Concat(db.DomainTypes.Select(x => (DbObject)x))
            .Concat(db.EnumTypes.Select(x => (DbObject)x))
            .Concat(db.RangeTypes.Select(x => (DbObject)x));

        HashSet<string> userDefinedTypesNames = new();
        foreach (DbObject udt in userDefinedTypes)
            userDefinedTypesNames.Add(udt.Name);
        return userDefinedTypesNames;
    }

    public static bool IsStandardSqlType(string typeName, out string normalizedName)
    {
        normalizedName = TryGetNormalizedTypeName(typeName, out string baseName);
        if (normalizedName != null)
            return true;
        else
            return false;
    }

    public static string TryGetNormalizedTypeName(string typeName, out string baseName)
    {
        Match match = Regex.Match(
            typeName,
            $@"^(?<name>[\w_]*)(?:\s+(?<name2>varying|precision))?\s*{P}(?:\s+(?<tz>with|without)\s*time\s*zone)?\s*{A}$",
            RegexOptions.IgnoreCase);

        string name = match.Groups["name"].Value;
        string name2 = match.Groups["name2"].Value;
        string tz = match.Groups["tz"].Value;
        if (!match.Success
            || IsVaryingStr(name2) && !Regex.IsMatch(name, "^(?:bit|character)$", RegexOptions.IgnoreCase)
            || IsPrecisionStr(name2) && !Regex.IsMatch(name, "^double$", RegexOptions.IgnoreCase)
            || match.Groups["tz"].Success && !Regex.IsMatch(name, "^(?:time|timestamp)$", RegexOptions.IgnoreCase))
        {
            throw new Exception($"Invalid datatype name '{typeName}'");
        }

        string pa = "";
        if (match.Groups["precision"].Success)
            pa = pa + match.Groups["precision"].Value;
        if (match.Groups["array"].Success)
            pa = pa + match.Groups["array"].Value;

        baseName = TryGetStandardBaseName(name);
        if (baseName != null)
            return Regex.Replace(baseName + pa, @"\s", "").ToUpper();
        else
            return null;

        string TryGetStandardBaseName(string name)
        {
            string nameToUpper = name.ToUpper();
            if (typeof(PostgreSQLDataTypeNames).GetFields().Select(x => x.Name)
                .Except(new string[] { "TIME", "TIMESTAMP" })
                .Any(x => x == nameToUpper))
            {
                return nameToUpper;
            }

            switch (nameToUpper)
            {
                case "BOOLEAN":
                    return PostgreSQLDataTypeNames.BOOL;
                case "INT2":
                    return PostgreSQLDataTypeNames.SMALLINT;
                case "INT4":
                case "INTEGER":
                    return PostgreSQLDataTypeNames.INT;
                case "INT8":
                    return PostgreSQLDataTypeNames.BIGINT;
                case "NUMERIC":
                    return PostgreSQLDataTypeNames.DECIMAL;
                case "REAL":
                    return PostgreSQLDataTypeNames.FLOAT4;
                case "BPCHAR":
                    return PostgreSQLDataTypeNames.CHAR;
                case "BIT":
                    return IsVaryingStr(name2) ? PostgreSQLDataTypeNames.VARBIT : PostgreSQLDataTypeNames.BIT;
                case "CHARACTER":
                    return IsVaryingStr(name2) ? PostgreSQLDataTypeNames.VARCHAR : PostgreSQLDataTypeNames.CHAR;
                case "DOUBLE":
                    return IsPrecisionStr(name2) ? PostgreSQLDataTypeNames.FLOAT8 : null;
                case "TIME":
                    return IsWithTimeZoneStr(tz) ? PostgreSQLDataTypeNames.TIMETZ : PostgreSQLDataTypeNames.TIME;
                case "TIMESTAMP":
                    return IsWithTimeZoneStr(tz) ? PostgreSQLDataTypeNames.TIMESTAMPTZ : PostgreSQLDataTypeNames.TIMESTAMP;
                default:
                    return null;
            }
        }

        static bool IsWithTimeZoneStr(string value)
        {
            return Regex.IsMatch(value, @"^with$", RegexOptions.IgnoreCase);
        }

        static bool IsVaryingStr(string value)
        {
            return Regex.IsMatch(value, @"^varying$", RegexOptions.IgnoreCase);
        }

        static bool IsPrecisionStr(string value)
        {
            return Regex.IsMatch(value, @"^precision$", RegexOptions.IgnoreCase);
        }
    }
}
