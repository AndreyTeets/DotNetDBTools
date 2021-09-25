using System;
using System.IO;
using System.Linq;
using System.Reflection;
using DotNetDBTools.DefinitionParser.MSSQL;
using DotNetDBTools.Deploy;
using DotNetDBTools.Models.MSSQL;
using DotNetDBTools.UnitTests.TestHelpers;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.Deploy
{
    public class MSSQLPublishScriptGenerationTests
    {
        [Fact]
        public void Generate_MSSQLPublishScript_For_MSSQLSampleDB_CreatesCorrectScript_WhenCreatingV1()
        {
            MSSQLDeployManager deployManager = new(true, false);

            Assembly dbAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.GetName().Name == "DotNetDBTools.SampleDB.MSSQL");
            MSSQLDatabaseInfo database = MSSQLDefinitionParser.CreateDatabaseInfo(dbAssembly);
            MSSQLDatabaseInfo existingDatabase = new(null);

            string outputPath = @"./generated/Actual_MSSQLPublishScript_For_MSSQLSampleDB_WhenCreatingV1.sql";
            deployManager.GeneratePublishScript(database, existingDatabase, outputPath);

            string actualScript = File.ReadAllText(outputPath);
            string expectedScript = File.ReadAllText(@"TestData/Expected_MSSQLPublishScript_For_MSSQLSampleDB_WhenCreatingV1.sql");
            actualScript.NormalizeLineEndings().Should().Be(expectedScript.NormalizeLineEndings());
        }

        [Fact]
        public void Generate_MSSQLPublishScript_For_MSSQLSampleDB_CreatesCorrectScript_WhenUpdatingFromV1ToV2()
        {
            MSSQLDeployManager deployManager = new(true, false);

            Assembly dbAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.GetName().Name == "DotNetDBTools.SampleDB.MSSQL");
            Assembly dbAssemblyV2 = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.GetName().Name == "DotNetDBTools.SampleDBv2.MSSQL");

            string outputPath = @"./generated/Actual_MSSQLPublishScript_For_MSSQLSampleDB_WhenUpdatingFromV1ToV2.sql";
            deployManager.GeneratePublishScript(dbAssemblyV2, dbAssembly, outputPath);

            string actualScript = File.ReadAllText(outputPath);
            string expectedScript = File.ReadAllText(@"TestData/Expected_MSSQLPublishScript_For_MSSQLSampleDB_WhenUpdatingFromV1ToV2.sql");
            actualScript.NormalizeLineEndings().Should().Be(expectedScript.NormalizeLineEndings());
        }

        [Fact]
        public void Generate_MSSQLPublishScript_For_MSSQLSampleDB_CreatesCorrectScript_WhenExistingDbIsEqual()
        {
            MSSQLDeployManager deployManager = new(true, false);

            Assembly dbAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.GetName().Name == "DotNetDBTools.SampleDB.MSSQL");
            string outputPath1 = @"./generated/Actual_MSSQLPublishScript_For_MSSQLSampleDB_WhenUpdatingFromV1ToV1.sql";
            deployManager.GeneratePublishScript(dbAssembly, dbAssembly, outputPath1);
            string actualScript1 = File.ReadAllText(outputPath1);
            actualScript1.Should().Be("");

            Assembly dbAssemblyV2 = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.GetName().Name == "DotNetDBTools.SampleDBv2.MSSQL");
            string outputPath2 = @"./generated/Actual_MSSQLPublishScript_For_MSSQLSampleDB_WhenUpdatingFromV2ToV2.sql";
            deployManager.GeneratePublishScript(dbAssemblyV2, dbAssemblyV2, outputPath2);
            string actualScript2 = File.ReadAllText(outputPath2);
            actualScript2.Should().Be("");
        }
    }
}
