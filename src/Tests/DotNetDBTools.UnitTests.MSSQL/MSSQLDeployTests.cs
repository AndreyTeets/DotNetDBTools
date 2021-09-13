using System.IO;
using System.Reflection;
using DotNetDBTools.DefinitionParser.Agnostic;
using DotNetDBTools.DefinitionParser.MSSQL;
using DotNetDBTools.Deploy.MSSQL;
using DotNetDBTools.Description.Agnostic;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.MSSQL;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.MSSQL
{
    public class MSSQLDeployTests
    {
        [Fact]
        public void GenerateUpdateScript_MSSQLSampleDB_CreatesCorrectScript()
        {
            Assembly dbAssembly = Assembly.GetAssembly(typeof(SampleDB.MSSQL.Tables.MyTable1));
            MSSQLDeployManager deployManager = new();
            MSSQLDatabaseInfo database = MSSQLDefinitionParser.CreateDatabaseInfo(dbAssembly);
            MSSQLDatabaseInfo existingDatabase = new();
            string actualScript = deployManager.GenerateUpdateScript(database, existingDatabase)
                .Replace("\r\n", "\n").Trim();
            string expectedScript = File.ReadAllText(@"TestData\MSSQLSampleDB_ExpectedUpdateScript.sql")
                .Replace("\r\n", "\n").Trim();
            actualScript.Should().Be(expectedScript);
        }

        [Fact]
        public void GenerateDescription_AgnosticSampleDB_CreatesCorrectDescription()
        {
            string dbAssemblyPath = "../../../../../Samples/DotNetDBTools.SampleDB.Agnostic/bin/DbAssembly/DotNetDBTools.SampleDB.Agnostic.dll";
            AgnosticDatabaseInfo database = AgnosticDefinitionParser.CreateDatabaseInfo(dbAssemblyPath);
            string actualDescriptionCode = AgnosticDbDescriptionGenerator.GenerateDescription(database)
                .Replace("\r\n", "\n").Trim();
            string expectedDescriptionCode = File.ReadAllText(@"TestData\AgnosticSampleDB_ExpectedDescriptionCode.cs")
                .Replace("\r\n", "\n").Trim();
            actualDescriptionCode.Should().Be(expectedDescriptionCode);
        }
    }
}
