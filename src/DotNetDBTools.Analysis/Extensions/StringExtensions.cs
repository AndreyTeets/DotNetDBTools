using System.Text.RegularExpressions;

namespace DotNetDBTools.Analysis.Extensions;

public static class StringExtensions
{
    public static string NormalizeLineEndings(this string value)
    {
        return value.Replace("\r\n", "\n").Trim();
    }

    public static string ToNoWhiteSpace(this string value)
    {
        return Regex.Replace(value, @"\s+", "");
    }

    public static string ToSingleSpaces(this string value)
    {
        return Regex.Replace(value, @"\s+", " ");
    }
}
