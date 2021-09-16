using System.IO;
using System.Reflection;
using DotNetDBTools.TestsUtils;
using DotNetDBTools.DefinitionParser.MSSQL;
using DotNetDBTools.Deploy.MSSQL;
using DotNetDBTools.Models.MSSQL;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.MSSQL.Deploy
{
    public class MSSQLUpdateScriptGenerationTests
    {
        [Fact]
        public void Generate_MSSQLUpdateScript_For_MSSQLSampleDB_CreatesCorrectScript_WhenExistingDbIsEmpty()
        {
            Assembly dbAssembly = Assembly.GetAssembly(typeof(SampleDB.MSSQL.Tables.MyTable1));
            MSSQLDeployManager deployManager = new(true, false);
            MSSQLDatabaseInfo databaseInfo = MSSQLDefinitionParser.CreateDatabaseInfo(dbAssembly);
            MSSQLDatabaseInfo existingDatabaseInfo = new();
            string actualScript = deployManager.GenerateUpdateScript(databaseInfo, existingDatabaseInfo);
            string expectedScript = File.ReadAllText(@"TestData\Expected_MSSQLUpdateScript_For_MSSQLSampleDB_WhenExistingDbIsEmpty.sql");
            actualScript.NormalizeLineEndings().Should().Be(expectedScript.NormalizeLineEndings());
        }
    }
}
