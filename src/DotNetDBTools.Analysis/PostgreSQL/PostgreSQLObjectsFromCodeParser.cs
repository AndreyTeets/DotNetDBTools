using System;
using System.Text.RegularExpressions;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Analysis.PostgreSQL
{
    public static class PostgreSQLObjectsFromCodeParser
    {
        private static readonly RegexOptions s_regexOptions =
            RegexOptions.Singleline |
            RegexOptions.IgnoreCase |
            RegexOptions.CultureInvariant |
            RegexOptions.IgnorePatternWhitespace;

        private const string WS0 = @"(?>\s*)";
        private const string WS1 = @"(?>\s+)";
        private const string Identifier = @"(?>[""|\w|\d|_]+)";

        public static PostgreSQLFunction ParseFunction(string statement)
        {
            string pattern =
@$"^--FunctionID:\# (?<funcID> (?>[{{|}}|\-|\w|\d]{{32,38}}) ) \#\r?\n
(?<funcCode> (?> CREATE{WS1}FUNCTION{WS1} (?<funcName>{Identifier}){WS0}\( .+;$ ) )";
            Match match = Regex.Match(statement, pattern, s_regexOptions);
            if (match.Groups[0].Success)
            {
                PostgreSQLFunction func = new()
                {
                    ID = Guid.Parse(match.Groups["funcID"].Value),
                    Name = GetIdentifierName(match.Groups["funcName"].Value),
                    CodePiece = new CodePiece { Code = match.Groups["funcCode"].Value },
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
}
