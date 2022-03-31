using System.IO;
using System.Reflection;
using DotNetDBTools.Analysis.Core;
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
    public void Generate_Description_For_AgnosticSampleDB_CreatesCorrectDescription()
    {
        Assembly dbAssembly = TestDbAssembliesHelper.GetSampleDbAssembly("DotNetDBTools.SampleDB.Agnostic");
        Database database = new GenericDbModelFromDefinitionProvider().CreateDbModel(dbAssembly);
        string actualDescriptionCode = DbDescriptionGenerator.GenerateDescription(database);
        string expectedDescriptionCode = File.ReadAllText(@"TestData/Agnostic/Expected_Description_For_SampleDB.cs");
        actualDescriptionCode.NormalizeLineEndings().Should().Be(expectedDescriptionCode.NormalizeLineEndings());
    }
}
