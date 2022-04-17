using System;
using System.Text.RegularExpressions;
using DotNetDBTools.CodeParsing.Core.Models;

namespace DotNetDBTools.CodeParsing.PostgreSQL;

public static class PostgreSQLGetObjectInfoHelper
{
    private static readonly RegexOptions s_regexOptions =
        RegexOptions.Singleline |
        RegexOptions.IgnoreCase |
        RegexOptions.CultureInvariant |
        RegexOptions.IgnorePatternWhitespace;

    private const string WS0 = @"(?>\s*)";
    private const string WS1 = @"(?>\s+)";
    private const string Identifier = @"(?>[""|\w|\d|_]+)";

    public static FunctionInfo ParseFunction(string statement)
    {
        string pattern =
$@"^--FunctionID:\# (?<funcID> (?>[{{|}}|\-|\w|\d]{{32,38}}) ) \#\r?\n
(?<funcCode> (?> CREATE{WS1}FUNCTION{WS1} (?<funcName>{Identifier}){WS0}\( .+;$ ) )";
        Match match = Regex.Match(statement, pattern, s_regexOptions);
        if (match.Groups[0].Success)
        {
            FunctionInfo func = new()
            {
                ID = Guid.Parse(match.Groups["funcID"].Value),
                Name = GetIdentifierName(match.Groups["funcName"].Value),
                Code = match.Groups["funcCode"].Value,
            };
            return func;
        }
        else
        {
            throw new Exception($"Failed to parse function from statement [{statement}]");
        }
    }

    private static string GetIdentifierName(string value)
    {
        return value.Replace("\"", "");
    }
}
