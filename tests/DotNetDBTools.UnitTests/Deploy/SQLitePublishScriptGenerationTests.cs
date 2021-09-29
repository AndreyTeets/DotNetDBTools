using System;
using System.IO;
using System.Linq;
using System.Reflection;
using DotNetDBTools.Deploy;
using DotNetDBTools.UnitTests.TestHelpers;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.Deploy
{
    public class SQLitePublishScriptGenerationTests
    {
        [Fact]
        public void Generate_SQLitePublishScript_For_SQLiteSampleDB_CreatesCorrectScript_WhenCreatingV1()
        {
            SQLiteDeployManager deployManager = new(true, false);

            Assembly dbAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.GetName().Name == "DotNetDBTools.SampleDB.SQLite");

            string outputPath = @"./generated/Actual_SQLitePublishScript_For_SQLiteSampleDB_WhenCreatingV1.sql";
            deployManager.GeneratePublishScript(dbAssembly, outputPath);

            string actualScript = File.ReadAllText(outputPath);
            string expectedScript = File.ReadAllText(@"TestData/Expected_SQLitePublishScript_For_SQLiteSampleDB_WhenCreatingV1.sql");
            actualScript.NormalizeLineEndings().Should().Be(expectedScript.NormalizeLineEndings());
        }

        [Fact]
        public void Generate_SQLitePublishScript_For_SQLiteSampleDB_CreatesCorrectScript_WhenUpdatingFromV1ToV2()
        {
            SQLiteDeployManager deployManager = new(true, false);

            Assembly dbAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.GetName().Name == "DotNetDBTools.SampleDB.SQLite");
            Assembly dbAssemblyV2 = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.GetName().Name == "DotNetDBTools.SampleDBv2.SQLite");

            string outputPath = @"./generated/Actual_SQLitePublishScript_For_SQLiteSampleDB_WhenUpdatingFromV1ToV2.sql";
            deployManager.GeneratePublishScript(dbAssemblyV2, dbAssembly, outputPath);

            string actualScript = File.ReadAllText(outputPath);
            string expectedScript = File.ReadAllText(@"TestData/Expected_SQLitePublishScript_For_SQLiteSampleDB_WhenUpdatingFromV1ToV2.sql");
            actualScript.NormalizeLineEndings().Should().Be(expectedScript.NormalizeLineEndings());
        }

        [Fact]
        public void Generate_SQLitePublishScript_For_SQLiteSampleDB_CreatesCorrectScript_WhenExistingDbIsEqual()
        {
            SQLiteDeployManager deployManager = new(true, false);

            Assembly dbAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.GetName().Name == "DotNetDBTools.SampleDB.SQLite");
            string outputPath1 = @"./generated/Actual_SQLitePublishScript_For_SQLiteSampleDB_WhenUpdatingFromV1ToV1.sql";
            deployManager.GeneratePublishScript(dbAssembly, dbAssembly, outputPath1);
            string actualScript1 = File.ReadAllText(outputPath1);
            actualScript1.Should().Be("");

            Assembly dbAssemblyV2 = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.GetName().Name == "DotNetDBTools.SampleDBv2.SQLite");
            string outputPath2 = @"./generated/Actual_SQLitePublishScript_For_SQLiteSampleDB_WhenUpdatingFromV2ToV2.sql";
            deployManager.GeneratePublishScript(dbAssemblyV2, dbAssemblyV2, outputPath2);
            string actualScript2 = File.ReadAllText(outputPath2);
            actualScript2.Should().Be("");
        }
    }
}
