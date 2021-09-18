using System.IO;
using System.Reflection;
using DotNetDBTools.DefinitionParser.Agnostic;
using DotNetDBTools.Description.Agnostic;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.UnitTests.TestHelpers;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.Description
{
    public class AgnosticDescriptionGenerationTests
    {
        [Fact]
        public void Generate_Description_For_AgnosticSampleDB_CreatesCorrectDescription()
        {
            Assembly dbAssembly = Assembly.GetAssembly(typeof(SampleDB.Agnostic.Tables.MyTable1));
            AgnosticDatabaseInfo databaseInfo = AgnosticDefinitionParser.CreateDatabaseInfo(dbAssembly);
            string actualDescriptionCode = AgnosticDescriptionSourceGenerator.GenerateDescription(databaseInfo);
            string expectedDescriptionCode = File.ReadAllText(@"TestData\Expected_Description_For_AgnosticSampleDB.cs");
            actualDescriptionCode.NormalizeLineEndings().Should().Be(expectedDescriptionCode.NormalizeLineEndings());
        }
    }
}
