using System.Reflection;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Generation;
using DotNetDBTools.Models.Core;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.Generation.Base;

public abstract class BaseDefinitionGenerationTests
{
    protected abstract string SpecificDbmsSampleDbV1AssemblyName { get; }
    protected abstract string SpecificDbmsSampleDbV2AssemblyName { get; }

    private static string GeneratedFilesDir => "./DefinitionGenerationTests_Generated";

    [Fact]
    public void DbModelFromGeneratedCSharpDefinition_IsEquivalentTo_DbModelFromOriginalDefinition()
    {
        DbModelFromGeneratedDefinition_IsEquivalentTo_DbModelFromOriginalDefinition_TestCase(
            SpecificDbmsSampleDbV1AssemblyName, OutputDefinitionKind.CSharp);

        DbModelFromGeneratedDefinition_IsEquivalentTo_DbModelFromOriginalDefinition_TestCase(
            SpecificDbmsSampleDbV2AssemblyName, OutputDefinitionKind.CSharp);
    }

    protected void DbModelFromGeneratedDefinition_IsEquivalentTo_DbModelFromOriginalDefinition_TestCase(
        string sampleDbAssemblyName, OutputDefinitionKind outputDefinitionKind)
    {
        Assembly origDefDbAssembly = MiscHelper.GetSampleDbAssembly(sampleDbAssemblyName);
        Database origDefDbModel = new DefinitionParsingManager().CreateDbModel(origDefDbAssembly);

        string projectDir = $@"{GeneratedFilesDir}/{sampleDbAssemblyName}_Generated{outputDefinitionKind}Definition";
        GenerationOptions options = new()
        {
            DatabaseName = sampleDbAssemblyName,
            OutputDefinitionKind = outputDefinitionKind,
        };
        new GenerationManager(options).GenerateDefinition(origDefDbModel, projectDir);
        Assembly genDefDbAssembly = TestDatabasesCompiler.CompileSampleDbProject(projectDir);
        Database genDefDbModel = new DefinitionParsingManager().CreateDbModel(genDefDbAssembly);
        genDefDbModel.Version = origDefDbModel.Version;

        genDefDbModel.Should().BeEquivalentTo(origDefDbModel, options => options
            .RespectingRuntimeTypes()
            .WithStrictOrdering()
            .Excluding(database => database.Scripts)
            .Excluding(mi => mi.Name == nameof(DbObject.Parent) && mi.DeclaringType == typeof(DbObject))
            .Excluding(mi => mi.Name == nameof(CodePiece.DependsOn) && mi.DeclaringType == typeof(CodePiece))
            .Excluding(mi => mi.Name == nameof(DataType.DependsOn) && mi.DeclaringType == typeof(DataType)));
    }
}
