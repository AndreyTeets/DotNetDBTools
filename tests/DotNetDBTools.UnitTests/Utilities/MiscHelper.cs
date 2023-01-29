using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using DotNetDBTools.Models.Core;
using FluentAssertions.Equivalency;
using Newtonsoft.Json;

namespace DotNetDBTools.UnitTests.Utilities;

internal static class MiscHelper
{
    public static Assembly GetSampleDbAssembly(string projectName)
    {
        string projectDir = $@"../../../../../samples/Databases/{projectName}";
        return TestDatabasesCompiler.CompileSampleDbProject(projectDir);
    }

    public static EquivalencyAssertionOptions<Database> ExcludingDependencies(this EquivalencyAssertionOptions<Database> options)
    {
        return options
            .Excluding(mi => mi.Name == nameof(DbObject.Parent) && mi.DeclaringType == typeof(DbObject))
            .Excluding(mi => mi.Name == nameof(CodePiece.DependsOn) && mi.DeclaringType == typeof(CodePiece))
            .Excluding(mi => mi.Name == nameof(DataType.DependsOn) && mi.DeclaringType == typeof(DataType))
            .Excluding(mi => mi.Name == nameof(PrimaryKey.DependsOn) && mi.DeclaringType == typeof(PrimaryKey))
            .Excluding(mi => mi.Name == nameof(UniqueConstraint.DependsOn) && mi.DeclaringType == typeof(UniqueConstraint))
            .Excluding(mi => mi.Name == nameof(ForeignKey.DependsOn) && mi.DeclaringType == typeof(ForeignKey))
            .Excluding(mi => mi.Name == nameof(Index.DependsOn) && mi.DeclaringType == typeof(Index));
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
        if (input.EndsWith(";", System.StringComparison.OrdinalIgnoreCase))
            return input.Remove(input.Length - 1, 1);
        else
            return input;
    }

    public static string NormalizeLineEndings(this string value)
    {
        return value.Replace("\r\n", "\n").Trim();
    }
}
