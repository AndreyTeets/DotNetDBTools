namespace DotNetDBTools.DefinitionParsing.Core
{
    public static class StringExtensions
    {
        public static string NormalizeLineEndings(this string value)
        {
            return value.Replace("\r\n", "\n").Trim();
        }
    }
}
