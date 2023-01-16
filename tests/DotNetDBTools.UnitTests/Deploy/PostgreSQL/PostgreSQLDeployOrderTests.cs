using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.CodeParsing;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Deploy;
using DotNetDBTools.Models;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.Deploy.PostgreSQL;

public class PostgreSQLDeployOrderTests
{
    private const string TestDataDir = "./TestData/PostgreSQL/DeployOrder";
    private readonly PostgreSQLDeployManager _deployManager = new();

    [Fact]
    public void CreateDatabase_GeneratesDDLStatements_InCorrectOrder()
    {
        PostgreSQLDatabase database = CreateDeployOrderTestsDatabaseModel();
        string createScript = _deployManager.GenerateNoDNDBTInfoPublishScript(database);

        string expectedCreateScript = MiscHelper.ReadFromFile($"{TestDataDir}/ExpectedCreateScript.sql");
        createScript.Should().Be(expectedCreateScript);
    }

    [Fact]
    public void UpdateDatabase_ControllablyThrows_OnRecursiveDependencies()
    {
        PostgreSQLDatabase originalDb = CreateDeployOrderTestsDatabaseModel();

        PostgreSQLDatabase newDb = originalDb.CopyModel();
        PostgreSQLFunction x = newDb.Functions.Single(x => x.Name == "f_3_s");
        x.CreateStatement.Code = x.CreateStatement.Code.Replace("(3 + 3)", "f_2_s(3)");
        x.CreateStatement.DependsOn.Add(newDb.Functions.Single(x => x.Name == "f_2_s"));
        newDb.Version++;

        FluentActions.Invoking(() =>
        {
            _deployManager.GenerateNoDNDBTInfoPublishScript(newDb, originalDb);
        }).Should().Throw<Exception>()
        .WithMessage($"Invalid objects dependencies graph, probably with cyclic dependency");
    }

    [Theory]
    [InlineData("s_1", "No objects depending on sequence are altered/recreated when sequence is modified")]
    [InlineData("s_1-2", "All required objects depending on sequence are altered/recreated in correct order when it is recreated")]
    [InlineData("f_3_s", "All required objects depending on sql function are altered/recreated in correct order when it is changed")]
    [InlineData("f_9_p", "All required objects depending on plpgsql function are altered/recreated in correct order when it is changed")]
    [InlineData("tp_3", "No objects depending on domain type are altered/recreated when it is renamed or" +
        " when only it's constraint is changed and there are no columns that depend on it through complex type")]
    [InlineData("tp_30", "All required objects depending on domain type are altered/recreated in correct order" +
        " when only it's constraint is changed but there are columns that depend on it through complex type")]
    [InlineData("tp_9", "All required objects depending on type are altered/recreated in correct order when it is changed")]
    [InlineData("t_1", "All required objects depending on table column are altered/recreated in correct order when data type is changed")]
    [InlineData("v_1", "All required objects depending on view are altered/recreated in correct order when it is changed")]
    [InlineData("i_a_1", "Index gets recreated when referenced in expression function is changed")]
    [InlineData("tr_a_1", "Trigger gets recreated when referenced in definition function is changed")]
    public void UpdateDatabase_GeneratesOnlyRequiredDDLStatements_AndInCorrectOrder(string caseName, string caseDescription)
    {
        switch (caseName)
        {
            case "s_1":
                TestCase(db =>
                {
                    PostgreSQLSequence x = db.Sequences.Single(x => x.Name == "s_1");
                    x.Name = "s_1x";
                    x.Options.MinValue = -777;
                    x.OwnedBy = ("t_1", "c1");
                }, caseName, caseDescription);
                break;
            case "s_1-2":
                TestCase(db =>
                {
                    PostgreSQLSequence x = db.Sequences.Single(x => x.Name == "s_1");
                    x.ID = new Guid("35A9566C-7088-4C47-B39C-3812ABDCD725");
                }, caseName, caseDescription);
                break;
            case "f_3_s":
                TestCase(db =>
                {
                    PostgreSQLFunction x = db.Functions.Single(x => x.Name == "f_3_s");
                    x.CreateStatement.Code = x.CreateStatement.Code.Replace("3 + 3", "3 + 4");
                }, caseName, caseDescription);
                break;
            case "f_9_p":
                TestCase(db =>
                {
                    PostgreSQLFunction x = db.Functions.Single(x => x.Name == "f_9_p");
                    x.CreateStatement.Code = x.CreateStatement.Code.Replace("7 + 7", "6 + 8");
                }, caseName, caseDescription);
                break;
            case "tp_3":
                TestCase(db =>
                {
                    PostgreSQLDomainType x = db.DomainTypes.Single(x => x.Name == "tp_3");
                    x.Name = "tp_3x";
                    x.Default.Code = "444";
                    x.CheckConstraints.Single().Expression.Code = "value != 9";
                }, caseName, caseDescription);
                break;
            case "tp_30":
                TestCase(db =>
                {
                    PostgreSQLDomainType x = db.DomainTypes.Single(x => x.Name == "tp_30");
                    x.CheckConstraints.Single().Expression.Code = "value != 39";
                }, caseName, caseDescription);
                break;
            case "tp_9":
                TestCase(db =>
                {
                    PostgreSQLCompositeType x = db.CompositeTypes.Single(x => x.Name == "tp_9");
                    x.Attributes.Single().DataType.Name = "BIGINT";
                }, caseName, caseDescription);
                break;
            case "t_1":
                TestCase(db =>
                {
                    Table x = db.Tables.Single(x => x.Name == "t_1");
                    x.Columns.Single().DataType.Name = "SMALLINT";
                }, caseName, caseDescription);
                break;
            case "v_1":
                TestCase(db =>
                {
                    View x = db.Views.Single(x => x.Name == "v_1");
                    x.CreateStatement.Code = x.CreateStatement.Code.Replace("8 + 8", "5 + 9");
                }, caseName, caseDescription);
                break;
            case "i_a_1":
                TestCase(db =>
                {
                    PostgreSQLFunction x = db.Functions.Single(x => x.Name == "f_7_s");
                    x.CreateStatement.Code = x.CreateStatement.Code.Replace("nextval('s_1')", "112");
                }, caseName, caseDescription);
                break;
            case "tr_a_1":
                TestCase(db =>
                {
                    PostgreSQLFunction x = db.Functions.Single(x => x.Name == "f_5_p");
                    x.CreateStatement.Code = x.CreateStatement.Code.Replace("f_2_s(8)", "118");
                }, caseName, caseDescription);
                break;
            default:
                throw new Exception($"Invalid caseName '{caseName}'");
        }

        void TestCase(Action<PostgreSQLDatabase> changeDbAction, string caseName, string caseDescription)
        {
            PostgreSQLDatabase originalDb = CreateDeployOrderTestsDatabaseModel();

            PostgreSQLDatabase newDb = originalDb.CopyModel();
            changeDbAction(newDb);
            newDb.Version++;

            string updateScript = _deployManager.GenerateNoDNDBTInfoPublishScript(newDb, originalDb);

            string expectedUpdateScript = MiscHelper.ReadFromFile($"{TestDataDir}/ExpectedUpdateScript-{caseName}.sql");
            updateScript.Should().Be(expectedUpdateScript, $"case '{caseName}' states '{caseDescription}'");
        }
    }

    private PostgreSQLDatabase CreateDeployOrderTestsDatabaseModel()
    {
        string statementsStr = MiscHelper.ReadFromFile($"{TestDataDir}/DatabaseDefinition.sql");
        List<string> statements = PostgreSQLStatementsSplitter.Split(statementsStr);
        Database database = new DefinitionParsingManager().CreateDbModel(statements, 1, DatabaseKind.PostgreSQL);
        return (PostgreSQLDatabase)database;
    }
}
