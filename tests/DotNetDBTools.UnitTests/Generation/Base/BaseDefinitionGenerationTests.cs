using System.Reflection;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Generation;
using DotNetDBTools.Models.Core;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.Generation.Base;

public abstract class BaseDefinitionGenerationTests<TDatabase>
    where TDatabase : Database
{
    protected abstract string SpecificDbmsSampleDbV1AssemblyName { get; }
    protected abstract string SpecificDbmsSampleDbV2AssemblyName { get; }

    private static string GeneratedFilesDir => "./generated";

    [Fact]
    public void DbModelFromGeneratedDefinition_IsEquivalentTo_DbModelFromDbAssembly()
    {
        TestCase(SpecificDbmsSampleDbV1AssemblyName);
        TestCase(SpecificDbmsSampleDbV2AssemblyName);

        void TestCase(string sampleDbAssemblyName)
        {
            Assembly initialDbAssembly = TestDbAssembliesHelper.GetSampleDbAssembly(sampleDbAssemblyName);
            TDatabase originalDbModel = (TDatabase)new GenericDbModelFromDefinitionProvider().CreateDbModel(initialDbAssembly);

            string projectDir = $@"{GeneratedFilesDir}/{sampleDbAssemblyName}_GeneratedDefinition";
            DbDefinitionGenerator.GenerateDefinition(originalDbModel, projectDir);
            Assembly genDefDbAssembly = TestDatabasesCompiler.CompileSampleDbProject(projectDir);
            TDatabase genDefDbModel = (TDatabase)new GenericDbModelFromDefinitionProvider().CreateDbModel(genDefDbAssembly);
            genDefDbModel.Version = originalDbModel.Version;

            genDefDbModel.Should().BeEquivalentTo(originalDbModel, options =>
                options.WithStrictOrdering()
                .Excluding(database => database.Name)
                .Excluding(database => database.Scripts));
        }
    }
}
