using System.IO;
using System.Reflection;
using DotNetDBTools.DefinitionParser.Agnostic;
using DotNetDBTools.DefinitionParser.SQLite;
using DotNetDBTools.Deploy.SQLite;
using DotNetDBTools.Description.Agnostic;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.SQLite;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.SQLite
{
    public class SQLiteDeployTests
    {
        [Fact]
        public void GenerateUpdateScript_SQLiteSampleDB_CreatesCorrectScript()
        {
            Assembly dbAssembly = Assembly.GetAssembly(typeof(SampleDB.SQLite.Tables.MyTable1));
            SQLiteDeployManager deployManager = new();
            SQLiteDatabaseInfo database = SQLiteDefinitionParser.CreateDatabaseInfo(dbAssembly);
            SQLiteDatabaseInfo existingDatabase = new();
            string actualScript = deployManager.GenerateUpdateScript(database, existingDatabase)
                .Replace("\r\n", "\n").Trim();
            string expectedScript = File.ReadAllText(@"TestData\SQLiteSampleDB_ExpectedUpdateScript.sql")
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
