using System.IO;
using System.Reflection;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Generation.Agnostic;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.Description.Agnostic
{
    public class AgnosticDescriptionGenerationTests
    {
        [Fact]
        public void Generate_Description_For_AgnosticSampleDB_CreatesCorrectDescription()
        {
            Assembly dbAssembly = TestDbAssembliesHelper.GetSampleDbAssembly("DotNetDBTools.SampleDB.Agnostic");
            AgnosticDatabase database = (AgnosticDatabase)DbDefinitionParser.CreateDatabaseModel(dbAssembly);
            string actualDescriptionCode = AgnosticDescriptionGenerator.GenerateDescription(database);
            string expectedDescriptionCode = File.ReadAllText(@"TestData/Agnostic/Expected_Description_For_SampleDB.cs");
            actualDescriptionCode.NormalizeLineEndings().Should().Be(expectedDescriptionCode.NormalizeLineEndings());
        }
    }
}
