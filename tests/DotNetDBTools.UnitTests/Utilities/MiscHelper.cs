using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using DotNetDBTools.Models.Core;
using Newtonsoft.Json;

namespace DotNetDBTools.UnitTests.Utilities;

internal static class MiscHelper
{
    public static Assembly GetSampleDbAssembly(string projectName)
    {
        string projectDir = $@"../../../../../samples/Databases/{projectName}";
        return TestDatabasesCompiler.CompileSampleDbProject(projectDir);
    }

    public static string SerializeToJsonWithReferences<T>(T value)
    {
        JsonDbModelReferenceResolver referenceResolver = new();
        JsonSerializerSettings jsonSettings = new()
        {
            Formatting = Formatting.Indented,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ContractResolver = new JsonDbModelContractResolver(),
            ReferenceResolverProvider = () => referenceResolver,
        };
        return JsonConvert.SerializeObject(value, jsonSettings).Replace("\r\n", "\n").Trim();
    }

    public static string GetDbObjectDisplayText(DbObject dbObject)
    {
        return $"DbObject {{ ID: '{dbObject.ID}', Name: '{dbObject.Name}' }}";
    }

    public static string ReadFromFile(string filePath)
    {
        return File.ReadAllText(filePath).NormalizeLineEndings();
    }

    public static string ReadFromFileWithoutIdDeclarations(string filePath)
    {
        string fileContent = ReadFromFile(filePath);
        return RemoveIdDeclarations(RemoveSemicolonIfAny(fileContent));
    }

    public static string RemoveIdDeclarations(this string input)
    {
        return Regex.Replace(input, @"--ID:#{[\w|-]{36}}#\r?\n", "");
    }

    public static string RemoveSemicolonIfAny(this string input)
    {
        if (input.EndsWith(";", StringComparison.OrdinalIgnoreCase))
            return input.Remove(input.Length - 1, 1);
        else
            return input;
    }

    public static string NormalizeLineEndings(this string value)
    {
        return value.Replace("\r\n", "\n").Trim();
    }
}
