using System;
using System.Text.RegularExpressions;
using DotNetDBTools.CodeParsing.Models;
using DotNetDBTools.UnitTests.Utilities;

namespace DotNetDBTools.UnitTests.CodeParsing.Base;

public abstract class BaseCodeParserTestsData
{
    public abstract string TestDataDir { get; }
    public abstract TableInfo ExpectedTable { get; }
    public abstract ViewInfo ExpectedView { get; }
    public abstract IndexInfo ExpectedIndex { get; }
    public abstract TriggerInfo ExpectedTrigger { get; }

    protected static string ReadStatementFromFileWithoutIdDeclarations(string filePath)
    {
        string fileContent = FilesHelper.GetFromFile(filePath);
        return RemoveIdDeclarations(RemoveSemicolonIfAny(fileContent));
    }

    private static string RemoveIdDeclarations(string input)
    {
        return Regex.Replace(input, @"--ID:#{[\w|-]{36}}#\r?\n", "");
    }

    private static string RemoveSemicolonIfAny(string input)
    {
        if (input.EndsWith(";", StringComparison.OrdinalIgnoreCase))
            return input.Remove(input.Length - 1, 1);
        else
            return input;
    }
}
