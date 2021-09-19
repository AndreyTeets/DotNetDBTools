using System.IO;
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
        public void Generate_MSSQLPublishScript_For_MSSQLSampleDB_CreatesCorrectScript_WhenExistingDbIsEmpty()
        {
            Assembly dbAssembly = Assembly.GetAssembly(typeof(SampleDB.MSSQL.Tables.MyTable1));
            MSSQLDeployManager deployManager = new(true, false);
            MSSQLDatabaseInfo databaseInfo = MSSQLDefinitionParser.CreateDatabaseInfo(dbAssembly);
            MSSQLDatabaseInfo existingDatabaseInfo = new(null);
            string outputPath = @"./generated/Actual_MSSQLPublishScript_For_MSSQLSampleDB_WhenExistingDbIsEmpty.sql";
            deployManager.GeneratePublishScript(databaseInfo, existingDatabaseInfo, outputPath);

            string actualScript = File.ReadAllText(outputPath);
            string expectedScript = File.ReadAllText(@"TestData\Expected_MSSQLPublishScript_For_MSSQLSampleDB_WhenExistingDbIsEmpty.sql");
            actualScript.NormalizeLineEndings().Should().Be(expectedScript.NormalizeLineEndings());
        }
    }
}
