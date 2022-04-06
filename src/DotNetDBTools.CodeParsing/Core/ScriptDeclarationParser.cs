using System;
using System.Text.RegularExpressions;
using DotNetDBTools.CodeParsing.Core.Models;

namespace DotNetDBTools.CodeParsing.Core;

public static class ScriptDeclarationParser
{
    private static readonly RegexOptions s_regexOptions =
        RegexOptions.Singleline |
        RegexOptions.IgnoreCase |
        RegexOptions.CultureInvariant |
        RegexOptions.IgnorePatternWhitespace;

    private const string WS0 = @"(?> \s* )";
    private const string AnyGuid = @"(?> [\-|\w|\d]{32,36} )";
    private const string AnyScriptName = @"(?> [\w|\d|_]+ )";
    private const string AnyScriptType = @"(?> BeforePublishOnce|AfterPublishOnce )";
    private const string AnyInt64 = @"(?> [\d]{1,19} )";
    private const string AnyText = @"(?> .*$ )";

    public static bool TryParseScriptInfo(string input, out ScriptInfo scriptInfo)
    {
        string pattern =
$@"^--ScriptID:\#{{ (?<scriptID> {AnyGuid} ) }}\#\r?\n
--ScriptName:\#{{ (?<scriptName> {AnyScriptName} ) }}\#\r?\n
--ScriptType:\#{{ (?<scriptType> {AnyScriptType} ) }}\#\r?\n
--ScriptMinDbVersionToExecute:\#{{ (?<scriptMinDbVersionToExecute> {AnyInt64} ) }}\#\r?\n
--ScriptMaxDbVersionToExecute:\#{{ (?<scriptMaxDbVersionToExecute> {AnyInt64} ) }}\#\r?\n
(?<scriptCode> {AnyText} )";

        Match match = Regex.Match(input, pattern, s_regexOptions);
        if (match.Groups[0].Success)
        {
            scriptInfo = new()
            {
                ID = Guid.Parse(match.Groups["scriptID"].Value),
                Name = match.Groups["scriptName"].Value,
                Type = (ScriptType)Enum.Parse(typeof(ScriptType), match.Groups["scriptType"].Value),
                MinDbVersionToExecute = long.Parse(match.Groups["scriptMinDbVersionToExecute"].Value),
                MaxDbVersionToExecute = long.Parse(match.Groups["scriptMaxDbVersionToExecute"].Value),
                Code = match.Groups["scriptCode"].Value,
            };
            return true;
        }
        else
        {
            string malformedInputPattern = $@"^{WS0}--ScriptID:\# {AnyText}$";
            if (Regex.IsMatch(input, malformedInputPattern, s_regexOptions))
                throw new Exception($"Failed to parse script info from input [{input}]");

            scriptInfo = null;
            return false;
        }
    }
}
