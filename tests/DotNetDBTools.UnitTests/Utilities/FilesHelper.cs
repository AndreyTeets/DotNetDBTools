using System.IO;
using DotNetDBTools.Analysis.Extensions;

namespace DotNetDBTools.UnitTests.Utilities;

internal static class FilesHelper
{
    public static string GetFromFile(string filePath)
    {
        return File.ReadAllText(filePath).NormalizeLineEndings();
    }
}
