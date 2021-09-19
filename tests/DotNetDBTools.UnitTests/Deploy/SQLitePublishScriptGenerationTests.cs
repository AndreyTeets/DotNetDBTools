using System.IO;
using System.Reflection;
using DotNetDBTools.DefinitionParser.SQLite;
using DotNetDBTools.Deploy;
using DotNetDBTools.Models.SQLite;
using DotNetDBTools.UnitTests.TestHelpers;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.Deploy
{
    public class SQLitePublishScriptGenerationTests
    {
        [Fact]
        public void Generate_SQLitePublishScript_For_SQLiteSampleDB_CreatesCorrectScript_WhenExistingDbIsEmpty()
        {
            Assembly dbAssembly = Assembly.GetAssembly(typeof(SampleDB.SQLite.Tables.MyTable1));
            SQLiteDeployManager deployManager = new(true, false);
            SQLiteDatabaseInfo databaseInfo = SQLiteDefinitionParser.CreateDatabaseInfo(dbAssembly);
            SQLiteDatabaseInfo existingDatabaseInfo = new(null);
            string outputPath = @"./generated/Actual_SQLitePublishScript_For_SQLiteSampleDB_WhenExistingDbIsEmpty.sql";
            deployManager.GeneratePublishScript(databaseInfo, existingDatabaseInfo, outputPath);

            string actualScript = File.ReadAllText(outputPath);
            string expectedScript = File.ReadAllText(@"TestData\Expected_SQLitePublishScript_For_SQLiteSampleDB_WhenExistingDbIsEmpty.sql");
            actualScript.NormalizeLineEndings().Should().Be(expectedScript.NormalizeLineEndings());
        }
    }
}
