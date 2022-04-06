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
    protected abstract string SampleDbV1CSharpAssemblyName { get; }
    protected abstract string GeneratedFilesDir { get; }

    private readonly Assembly _dbAssemblyV1CSharp;

    protected BaseDefinitionGenerationTests()
    {
        _dbAssemblyV1CSharp = TestDbAssembliesHelper.GetSampleDbAssembly(SampleDbV1CSharpAssemblyName);
    }

    [Fact]
    public void DbModelCreatedFrom_SampleDBv1_IsEquivalentTo_DbModelCreatedFrom_GeneratedDefinition()
    {
        TDatabase originalDbModel = (TDatabase)new GenericDbModelFromDefinitionProvider().CreateDbModel(_dbAssemblyV1CSharp);

        string projectDir = $@"{GeneratedFilesDir}/SampleDBv1GeneratedDefinition";
        DbDefinitionGenerator.GenerateDefinition(originalDbModel, projectDir);
        Assembly genDefDbAssembly = TestDatabasesCompiler.CompileSampleDbProject(projectDir);
        TDatabase genDefDbModel = (TDatabase)new GenericDbModelFromDefinitionProvider().CreateDbModel(genDefDbAssembly);

        genDefDbModel.Should().BeEquivalentTo(originalDbModel, options =>
            options.WithStrictOrdering()
            .Excluding(database => database.Name)
            .Excluding(database => database.Scripts));
    }
}
