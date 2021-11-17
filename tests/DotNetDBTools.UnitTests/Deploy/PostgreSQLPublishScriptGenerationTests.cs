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
    public class PostgreSQLPublishScriptGenerationTests
    {
        [Fact]
        public void Generate_PostgreSQLPublishScript_For_PostgreSQLSampleDB_CreatesCorrectScript_WhenCreatingV1()
        {
            PostgreSQLDeployManager deployManager = new(new DeployOptions());

            Assembly dbAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.GetName().Name == "DotNetDBTools.SampleDB.PostgreSQL");

            string outputPath = @"./generated/Actual_PostgreSQLPublishScript_For_PostgreSQLSampleDB_WhenCreatingV1.sql";
            deployManager.GeneratePublishScript(dbAssembly, outputPath);

            string actualScript = File.ReadAllText(outputPath);
            string expectedScript = File.ReadAllText(@"TestData/Expected_PostgreSQLPublishScript_For_PostgreSQLSampleDB_WhenCreatingV1.sql");
            actualScript.NormalizeLineEndings().Should().Be(expectedScript.NormalizeLineEndings());
        }

        [Fact]
        public void Generate_PostgreSQLPublishScript_For_PostgreSQLSampleDB_CreatesCorrectScript_WhenUpdatingFromV1ToV2()
        {
            PostgreSQLDeployManager deployManager = new(new DeployOptions { AllowDataLoss = true });

            Assembly dbAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.GetName().Name == "DotNetDBTools.SampleDB.PostgreSQL");
            Assembly dbAssemblyV2 = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.GetName().Name == "DotNetDBTools.SampleDBv2.PostgreSQL");

            string outputPath = @"./generated/Actual_PostgreSQLPublishScript_For_PostgreSQLSampleDB_WhenUpdatingFromV1ToV2.sql";
            deployManager.GeneratePublishScript(dbAssemblyV2, dbAssembly, outputPath);

            string actualScript = File.ReadAllText(outputPath);
            string expectedScript = File.ReadAllText(@"TestData/Expected_PostgreSQLPublishScript_For_PostgreSQLSampleDB_WhenUpdatingFromV1ToV2.sql");
            actualScript.NormalizeLineEndings().Should().Be(expectedScript.NormalizeLineEndings());
        }

        [Fact]
        public void Generate_PostgreSQLPublishScript_For_PostgreSQLSampleDB_CreatesCorrectScript_WhenExistingDbIsEqual()
        {
            PostgreSQLDeployManager deployManager = new(new DeployOptions());

            Assembly dbAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.GetName().Name == "DotNetDBTools.SampleDB.PostgreSQL");
            string outputPath1 = @"./generated/Actual_PostgreSQLPublishScript_For_PostgreSQLSampleDB_WhenUpdatingFromV1ToV1.sql";
            deployManager.GeneratePublishScript(dbAssembly, dbAssembly, outputPath1);
            string actualScript1 = File.ReadAllText(outputPath1);
            actualScript1.Should().Be("");

            Assembly dbAssemblyV2 = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.GetName().Name == "DotNetDBTools.SampleDBv2.PostgreSQL");
            string outputPath2 = @"./generated/Actual_PostgreSQLPublishScript_For_PostgreSQLSampleDB_WhenUpdatingFromV2ToV2.sql";
            deployManager.GeneratePublishScript(dbAssemblyV2, dbAssemblyV2, outputPath2);
            string actualScript2 = File.ReadAllText(outputPath2);
            actualScript2.Should().Be("");
        }
    }
}
