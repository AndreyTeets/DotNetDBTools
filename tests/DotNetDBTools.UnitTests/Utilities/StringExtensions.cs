namespace DotNetDBTools.UnitTests.Utilities
{
    public static class StringExtensions
    {
        public static string NormalizeLineEndings(this string value)
        {
            return value.Replace("\r\n", "\n").Trim();
        }
    }
}
