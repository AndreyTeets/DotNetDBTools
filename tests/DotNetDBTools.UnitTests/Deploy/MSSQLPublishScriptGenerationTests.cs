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
    public class MSSQLPublishScriptGenerationTests
    {
        [Fact]
        public void Generate_MSSQLPublishScript_For_MSSQLSampleDB_CreatesCorrectScript_WhenExistingDbIsEmpty()
        {
            Assembly dbAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.GetName().Name == "DotNetDBTools.SampleDB.MSSQL");
            Assembly dbAssemblyV2 = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.GetName().Name == "DotNetDBTools.SampleDBv2.MSSQL");

            MSSQLDeployManager deployManager = new(true, false);
            string outputPath = @"./generated/Actual_MSSQLPublishScript_For_MSSQLSampleDB_WhenUpdatingFromV1ToV2.sql";
            deployManager.GeneratePublishScript(dbAssemblyV2, dbAssembly, outputPath);

            string actualScript = File.ReadAllText(outputPath);
            string expectedScript = File.ReadAllText(@"TestData/Expected_MSSQLPublishScript_For_MSSQLSampleDB_WhenUpdatingFromV1ToV2.sql");
            actualScript.NormalizeLineEndings().Should().Be(expectedScript.NormalizeLineEndings());
        }
    }
}
