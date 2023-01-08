using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using PgDt = DotNetDBTools.Models.PostgreSQL.PostgreSQLDataTypeNames;

namespace DotNetDBTools.Analysis.PostgreSQL;

internal static class PostgreSQLHelperMethods
{
    private const string P = @"(?<precision>\([\s\d\,\-]+\))?";
    private const string A = @"(?<array>(?:\[[\s\d]*\]\s*)*)";
    private const string IF = @"YEAR|MONTH|DAY|HOUR|MINUTE|SECOND|YEAR\s*TO\s*MONTH|DAY\s*TO\s*HOUR"
        + @"|DAY\s*TO\s*MINUTE|DAY\s*TO\s*SECOND|HOUR\s*TO\s*MINUTE|HOUR\s*TO\s*SECOND|MINUTE\s*TO\s*SECOND";

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
            $@"^(?<name>[\w_]*)(?:\s+(?<name2>varying|precision|{IF}))?\s*{P}(?:\s+(?<tz>with|without)\s*time\s*zone)?\s*{A}$",
            RegexOptions.IgnoreCase);

        string name = match.Groups["name"].Value;
        string name2 = match.Groups["name2"].Value;
        string tz = match.Groups["tz"].Value;
        if (!match.Success
            || IsVaryingStr(name2) && !Regex.IsMatch(name, "^(?:bit|character)$", RegexOptions.IgnoreCase)
            || IsPrecisionStr(name2) && !Regex.IsMatch(name, "^double$", RegexOptions.IgnoreCase)
            || IsIntervalFieldsStr(name2) && !Regex.IsMatch(name, "^interval", RegexOptions.IgnoreCase)
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
        if (baseName == PgDt.INTERVAL && IsIntervalFieldsStr(name2))
            return $"{baseName} {name2.ToSingleSpaces().ToUpper()}{pa.ToNoWhiteSpace()}";
        else if (baseName != null)
            return (baseName + pa).ToNoWhiteSpace().ToUpper();
        else
            return null;

        string TryGetStandardBaseName(string name)
        {
            string nameToUpper = name.ToUpper();
            if (typeof(PgDt).GetFields().Select(x => x.Name)
                .Except(new string[] { PgDt.BIT, PgDt.INTERVAL, PgDt.TIME, PgDt.TIMESTAMP })
                .Any(x => x == nameToUpper))
            {
                return nameToUpper;
            }

            switch (nameToUpper)
            {
                case "BOOLEAN":
                    return PgDt.BOOL;
                case "INT2":
                    return PgDt.SMALLINT;
                case "INT4":
                case "INTEGER":
                    return PgDt.INT;
                case "INT8":
                    return PgDt.BIGINT;
                case "NUMERIC":
                    return PgDt.DECIMAL;
                case "REAL":
                    return PgDt.FLOAT4;
                case "BPCHAR":
                    return PgDt.CHAR;
                case PgDt.BIT:
                    return IsVaryingStr(name2) ? PgDt.VARBIT : nameToUpper;
                case "CHARACTER":
                    return IsVaryingStr(name2) ? PgDt.VARCHAR : PgDt.CHAR;
                case "DOUBLE":
                    return IsPrecisionStr(name2) ? PgDt.FLOAT8 : null;
                case PgDt.INTERVAL:
                    return nameToUpper;
                case PgDt.TIME:
                    return IsWithTimeZoneStr(tz) ? PgDt.TIMETZ : nameToUpper;
                case PgDt.TIMESTAMP:
                    return IsWithTimeZoneStr(tz) ? PgDt.TIMESTAMPTZ : nameToUpper;
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

        static bool IsIntervalFieldsStr(string value)
        {
            return Regex.IsMatch(value, $@"^(?:{IF})$", RegexOptions.IgnoreCase);
        }
    }
}
