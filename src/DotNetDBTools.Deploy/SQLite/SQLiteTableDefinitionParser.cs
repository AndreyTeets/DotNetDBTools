using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotNetDBTools.Deploy.SQLite
{
    internal static class SQLiteTableDefinitionParser
    {
        private static readonly RegexOptions s_regexOptions =
            RegexOptions.Singleline |
            RegexOptions.IgnoreCase |
            RegexOptions.CultureInvariant |
            RegexOptions.IgnorePatternWhitespace;

        private const string WS0 = @"(?>\s*)";
        private const string WS1 = @"(?>\s+)";
        private const string Identifier = @"(?>[\[|\]|""|`|\w|\d|_]+)";
        private static string Quote(string x) => @$"(?:{x}|\[{x}\]|""{x}""|`{x}`)";

        public static List<string> ParseToDefinitionStatements(string tableDefinition)
        {
            List<string> definitionStatements = new();
            string pattern = @$"CREATE{WS1}TABLE{WS1}{Identifier}{WS0}\( (?<Statements>.+) \)";
            Match match = Regex.Match(tableDefinition, pattern, s_regexOptions);
            if (!match.Groups["Statements"].Success)
                throw new Exception($"Failed to parse definition statements substring in [{tableDefinition}]");

            string unprocessedDefinitionStatementsStr = match.Groups["Statements"].Value.Trim() + ",";
            int statementLength;
            while ((statementLength = FindFirstStatementLength(unprocessedDefinitionStatementsStr)) > 0)
            {
                string statement = unprocessedDefinitionStatementsStr.Substring(0, statementLength).Trim();
                definitionStatements.Add(statement);
                unprocessedDefinitionStatementsStr = unprocessedDefinitionStatementsStr.Substring(statementLength + 1).Trim();
            }
            return definitionStatements;
        }

        public static List<(string ckName, string ckCode)> GetCheckConstraints(IEnumerable<string> definitionStatements)
        {
            List<(string ckName, string ckCode)> checkConstraints = new();
            string pattern = @$"^(?: CONSTRAINT{WS1}(?<ckName>{Identifier}){WS1} )? CHECK{WS0}\( (?<ckCode>.+) \)";
            foreach (string statement in definitionStatements)
            {
                Match match = Regex.Match(statement, pattern, s_regexOptions);
                if (match.Groups[0].Success)
                {
                    string ckName = GetIdentifierName(match.Groups["ckName"].Value);
                    string ckCode = "CHECK (" + match.Groups["ckCode"].Value + ")";
                    checkConstraints.Add((ckName, ckCode));
                }
            }
            return checkConstraints;
        }

        public static string GetUniqueConstraintName(IEnumerable<string> definitionStatements, IEnumerable<string> columns)
        {
            string columnsListExpr = @$"{WS0}{string.Join(@$"{WS0},{WS0}", columns.Select(x => Quote(x)))}{WS0}";
            string pattern = @$"^(?: CONSTRAINT{WS1}(?<ucName>{Identifier}){WS1} )? UNIQUE{WS0}\( {columnsListExpr} \)";
            foreach (string statement in definitionStatements)
            {
                Match match = Regex.Match(statement, pattern, s_regexOptions);
                if (match.Groups["ucName"].Success)
                    return GetIdentifierName(match.Groups["ucName"].Value);
                else if (match.Groups[0].Success)
                    return null;
            }
            throw new Exception($"Failed to find unique constraint name for columns [{string.Join(",", columns)}]");
        }

        public static string GetForeignKeyConstraintName(IEnumerable<string> definitionStatements, IEnumerable<string> thisColumns)
        {
            string thisColumnsListExpr = @$"{WS0}{string.Join(@$"{WS0},{WS0}", thisColumns.Select(x => Quote(x)))}{WS0}";
            string pattern = @$"^(?: CONSTRAINT{WS1}(?<fkName>{Identifier}){WS1} )? FOREIGN{WS1}KEY{WS0}\( {thisColumnsListExpr} \)";
            foreach (string statement in definitionStatements)
            {
                Match match = Regex.Match(statement, pattern, s_regexOptions);
                if (match.Groups["fkName"].Success)
                    return GetIdentifierName(match.Groups["fkName"].Value);
                else if (match.Groups[0].Success)
                    return null;
            }
            throw new Exception($"Failed to find foreign key name for thisColumns [{string.Join(",", thisColumns)}]");
        }

        private static int FindFirstStatementLength(string definitionStatements)
        {
            if (definitionStatements.Length == 0)
                return -1;

            bool insideQuotes = false;
            int parenthesesLevel = 0;

            int statementLength = 0;
            foreach (char c in definitionStatements)
            {
                if (c == '\'')
                    insideQuotes = !insideQuotes;
                else if (c == '(' && !insideQuotes)
                    parenthesesLevel++;
                else if (c == ')' && !insideQuotes && parenthesesLevel != 0)
                    parenthesesLevel--;
                else if (c == ',' && !insideQuotes && parenthesesLevel == 0)
                    return statementLength > 0 ? statementLength : throw NewFailedToFindFirstStatementException();

                statementLength++;
            }
            throw NewFailedToFindFirstStatementException();

            Exception NewFailedToFindFirstStatementException() =>
                new($"Failed to find first statement length in [{definitionStatements}]");
        }

        private static string GetIdentifierName(string value)
        {
            return value
                .Replace("[", "").Replace("]", "")
                .Replace("`", "")
                .Replace("\"", "");
        }
    }
}
