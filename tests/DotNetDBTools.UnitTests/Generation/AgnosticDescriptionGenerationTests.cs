using System;
using System.IO;
using System.Linq;
using System.Reflection;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Generation.Agnostic;
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
            Assembly dbAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.GetName().Name == "DotNetDBTools.SampleDB.Agnostic");
            AgnosticDatabase database = (AgnosticDatabase)DbDefinitionParser.CreateDatabaseModel(dbAssembly);
            string actualDescriptionCode = AgnosticDescriptionGenerator.GenerateDescription(database);
            string expectedDescriptionCode = File.ReadAllText(@"TestData/Expected_Description_For_AgnosticSampleDB.cs");
            actualDescriptionCode.NormalizeLineEndings().Should().Be(expectedDescriptionCode.NormalizeLineEndings());
        }
    }
}
