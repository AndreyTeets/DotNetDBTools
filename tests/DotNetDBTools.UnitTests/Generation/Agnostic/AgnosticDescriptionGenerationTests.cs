using System.Reflection;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Generation;
using DotNetDBTools.Models.Core;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.Generation.Agnostic;

public class AgnosticDescriptionGenerationTests
{
    [Fact]
    public void GenerateDescription_CreatesCorrectDescription()
    {
        Assembly dbAssembly = TestDbAssembliesHelper.GetSampleDbAssembly("DotNetDBTools.SampleDB.Agnostic");
        Database database = new DefinitionParsingManager().CreateDbModel(dbAssembly);
        GenerationOptions options = new() { DatabaseName = "DotNetDBToolsSampleDBAgnostic" };
        string actualDescriptionCode = new GenerationManager(options).GenerateDescription(database);
        string expectedDescriptionCode = FilesHelper.GetFromFile(@"TestData/Agnostic/Expected_Description_V1.cs");
        actualDescriptionCode.Should().Be(expectedDescriptionCode);
    }
}
