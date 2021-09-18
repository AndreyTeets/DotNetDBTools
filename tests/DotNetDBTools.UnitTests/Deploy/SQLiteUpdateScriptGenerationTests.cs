using System.IO;
using System.Reflection;
using DotNetDBTools.DefinitionParser.SQLite;
using DotNetDBTools.Deploy.SQLite;
using DotNetDBTools.Models.SQLite;
using DotNetDBTools.UnitTests.TestHelpers;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.Deploy
{
    public class SQLiteUpdateScriptGenerationTests
    {
        [Fact]
        public void Generate_SQLiteUpdateScript_For_SQLiteSampleDB_CreatesCorrectScript_WhenExistingDbIsEmpty()
        {
            Assembly dbAssembly = Assembly.GetAssembly(typeof(SampleDB.SQLite.Tables.MyTable1));
            SQLiteDeployManager deployManager = new(true, false);
            SQLiteDatabaseInfo databaseInfo = SQLiteDefinitionParser.CreateDatabaseInfo(dbAssembly);
            SQLiteDatabaseInfo existingDatabaseInfo = new(null);
            string actualScript = deployManager.GenerateUpdateScript(databaseInfo, existingDatabaseInfo);
            string expectedScript = File.ReadAllText(@"TestData\Expected_SQLiteUpdateScript_For_SQLiteSampleDB_WhenExistingDbIsEmpty.sql");
            actualScript.NormalizeLineEndings().Should().Be(expectedScript.NormalizeLineEndings());
        }
    }
}
