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
    public void DbModelFromGeneratedDefinition_IsEquivalentTo_DbModelFromOriginalDefinition()
    {
        TestCase(SpecificDbmsSampleDbV1AssemblyName);
        TestCase(SpecificDbmsSampleDbV2AssemblyName);

        void TestCase(string sampleDbAssemblyName)
        {
            Assembly origDefDbAssembly = TestDbAssembliesHelper.GetSampleDbAssembly(sampleDbAssemblyName);
            TDatabase origDefDbModel = (TDatabase)new GenericDbModelFromDefinitionProvider().CreateDbModel(origDefDbAssembly);

            string projectDir = $@"{GeneratedFilesDir}/{sampleDbAssemblyName}_GeneratedDefinition";
            DbDefinitionGenerator.GenerateDefinition(origDefDbModel, projectDir);
            Assembly genDefDbAssembly = TestDatabasesCompiler.CompileSampleDbProject(projectDir);
            TDatabase genDefDbModel = (TDatabase)new GenericDbModelFromDefinitionProvider().CreateDbModel(genDefDbAssembly);
            genDefDbModel.Version = origDefDbModel.Version;

            genDefDbModel.Should().BeEquivalentTo(origDefDbModel, options =>
                options.WithStrictOrdering()
                .Excluding(database => database.Scripts));
        }
    }
}
