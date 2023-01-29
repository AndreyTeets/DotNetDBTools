using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Deploy;
using DotNetDBTools.Models;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.Deploy.SQLite;

public class SQLiteDeployOrderTests
{
    private const string TestDataDir = "./TestData/SQLite/DeployOrder";
    private readonly SQLiteDeployManager _deployManager = new(new DeployOptions() { AllowDataLoss = true });

    [Fact]
    public void CreateDatabase_GeneratesDDLStatements_InCorrectOrder()
    {
        SQLiteDatabase database = CreateDeployOrderTestsDatabaseModel();
        string createScript = _deployManager.GenerateNoDNDBTInfoPublishScript(database);

        string expectedCreateScript = MiscHelper.ReadFromFile($"{TestDataDir}/ExpectedCreateScript.sql");
        createScript.Should().Be(expectedCreateScript);
    }

    [Theory]
    [InlineData("t_1", "All required objects depending on table are recreated in correct order when column data type is changed")]
    [InlineData("v_1", "All required objects depending on view are recreated in correct order when it is changed")]
    public void UpdateDatabase_GeneratesOnlyRequiredDDLStatements_AndInCorrectOrder(string caseName, string caseDescription)
    {
        switch (caseName)
        {
            case "t_1":
                TestCase(db =>
                {
                    Table x = db.Tables.Single(x => x.Name == "t_1");
                    x.Columns.Single(x => x.Name == "c1").DataType.Name = "BLOB";
                }, caseName, caseDescription);
                break;
            case "v_1":
                TestCase(db =>
                {
                    View x = db.Views.Single(x => x.Name == "v_1");
                    x.CreateStatement.Code = x.CreateStatement.Code.Replace("8 + 8", "5 + 9");
                }, caseName, caseDescription);
                break;
            default:
                throw new Exception($"Invalid caseName '{caseName}'");
        }

        void TestCase(Action<SQLiteDatabase> changeDbAction, string caseName, string caseDescription)
        {
            SQLiteDatabase originalDb = CreateDeployOrderTestsDatabaseModel();

            SQLiteDatabase newDb = originalDb.CopyModel();
            changeDbAction(newDb);
            newDb.Version++;

            string updateScript = _deployManager.GenerateNoDNDBTInfoPublishScript(newDb, originalDb);

            string expectedUpdateScript = MiscHelper.ReadFromFile($"{TestDataDir}/ExpectedUpdateScript-{caseName}.sql");
            updateScript.Should().Be(expectedUpdateScript, $"case '{caseName}' states '{caseDescription}'");
        }
    }

    private SQLiteDatabase CreateDeployOrderTestsDatabaseModel()
    {
        string statementsStr = MiscHelper.ReadFromFile($"{TestDataDir}/DatabaseDefinition.sql");
        List<string> statements = statementsStr.Split(";\n").ToList();
        Database database = new DefinitionParsingManager().CreateDbModel(statements, 1, DatabaseKind.SQLite);
        return (SQLiteDatabase)database;
    }
}
