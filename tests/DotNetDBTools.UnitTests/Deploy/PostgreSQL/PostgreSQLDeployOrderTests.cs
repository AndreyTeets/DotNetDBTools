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
    private readonly PostgreSQLDeployManager _deployManager = new(new DeployOptions() { AllowDataLoss = true });

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
    [InlineData("p_3_s", "All required objects depending on sql procedure are altered/recreated in correct order when it is changed")]
    [InlineData("p_9_p", "All required objects depending on plpgsql procedure are altered/recreated in correct order when it is changed")]
    [InlineData("tp_3", "No objects depending on domain type are altered/recreated when it is renamed or" +
        " when only it's constraint is changed and there are no columns that depend on it through complex type")]
    [InlineData("tp_30", "All required objects depending on domain type are altered/recreated in correct order" +
        " when only it's constraint is changed but there are columns that depend on it through complex type")]
    [InlineData("tp_9", "All required objects depending on type are altered/recreated in correct order when it is changed")]
    [InlineData("t_1", "All required objects depending on table column are altered/recreated in correct order when data type is changed")]
    [InlineData("t_1-2", "All required objects depending on table are recreated in correct order when it is recreated")]
    [InlineData("t_1-3", "No objects depending on table are recreated when unreferenced column is recreated")]
    [InlineData("v_1", "All required objects depending on view are altered/recreated in correct order when it is changed")]
    [InlineData("i_a_1", "Index gets recreated when referenced in expression function is changed")]
    [InlineData("tr_a_1", "Trigger gets recreated when referenced in definition function is changed")]
    [InlineData("d_u_1", "Domain gets renamed and does not get recreated when it's name is changed")]
    [InlineData("d_u_1-2", "Domain gets recreated when it's name is changed and at the same time underlying type is recreated")]
    [InlineData("d_u_2", "Domain default gets recreated when it's name is changed and at the same time default dependency is changed")]
    [InlineData("d_u_3", "Domain CK gets recreated when it's name is changed and at the same time CK dependency is changed")]
    [InlineData("t_u_4", "Table column gets renamed and does not get redefined when it's name is changed")]
    [InlineData("t_u_4-2", "Table column gets redefined when it's name is changed and at the same time it's data type is recreated")]
    [InlineData("t_u_5", "Table column default gets recreated when it's name is changed and at the same time default dependency is changed")]
    [InlineData("t_u_6", "Table CK gets recreated when it's name is changed and at the same time CK dependency is changed")]
    [InlineData("v_u_7", "View gets recreated when it's name is changed")]
    [InlineData("v_u_7-2", "View gets recreated when it's name is changed and at the same time dependency is changed")]
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
            case "p_3_s":
                TestCase(db =>
                {
                    PostgreSQLProcedure x = db.Procedures.Single(x => x.Name == "p_3_s");
                    x.CreateStatement.Code = x.CreateStatement.Code.Replace("33 + 33", "33 + 44");
                }, caseName, caseDescription);
                break;
            case "p_9_p":
                TestCase(db =>
                {
                    PostgreSQLProcedure x = db.Procedures.Single(x => x.Name == "p_9_p");
                    x.CreateStatement.Code = x.CreateStatement.Code.Replace("77 + 77", "66 + 88");
                }, caseName, caseDescription);
                break;
            case "tp_3":
                TestCase(db =>
                {
                    PostgreSQLDomainType x = db.DomainTypes.Single(x => x.Name == "tp_3");
                    x.Name = "tp_3x";
                    x.Default.Code = "444";
                    x.CheckConstraints.Single(x => x.Name == "ck_tp_3").Expression.Code = "value != 9";
                }, caseName, caseDescription);
                break;
            case "tp_30":
                TestCase(db =>
                {
                    PostgreSQLDomainType x = db.DomainTypes.Single(x => x.Name == "tp_30");
                    x.CheckConstraints.Single(x => x.Name == "ck_tp_30").Expression.Code = "value != 39";
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
                    x.Columns.Single(x => x.Name == "c1").DataType.Name = "SMALLINT";
                }, caseName, caseDescription);
                break;
            case "t_1-2":
                TestCase(db =>
                {
                    Table x = db.Tables.Single(x => x.Name == "t_1");
                    x.ID = new Guid("B3624592-76AF-4AD9-A999-6383B05CD794");
                }, caseName, caseDescription);
                break;
            case "t_1-3":
                TestCase(db =>
                {
                    Table x = db.Tables.Single(x => x.Name == "t_1");
                    x.Columns.Single(x => x.Name == "c2").ID = new Guid("8166C051-B5E3-4F71-918C-3469726E7BEF");
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
            case "d_u_1":
                TestCase(db =>
                {
                    PostgreSQLDomainType x = db.DomainTypes.Single(x => x.Name == "d_u_1");
                    x.Name = "d_u_1x";
                }, caseName, caseDescription);
                break;
            case "d_u_1-2":
                TestCase(db =>
                {
                    PostgreSQLDomainType x = db.DomainTypes.Single(x => x.Name == "d_u_1");
                    x.Name = "d_u_1x";
                    PostgreSQLEnumType y = db.EnumTypes.Single(x => x.Name == "tp_u_1");
                    y.AllowedValues = new() { "l1x" };
                }, caseName, caseDescription);
                break;
            case "d_u_2":
                TestCase(db =>
                {
                    PostgreSQLDomainType x = db.DomainTypes.Single(x => x.Name == "d_u_2");
                    x.Name = "d_u_2x";
                    PostgreSQLCompositeType y = db.CompositeTypes.Single(x => x.Name == "tp_u_2");
                    y.Attributes.Single().DataType.Name = "BIGINT";
                }, caseName, caseDescription);
                break;
            case "d_u_3":
                TestCase(db =>
                {
                    PostgreSQLDomainType x = db.DomainTypes.Single(x => x.Name == "d_u_3");
                    x.Name = "d_u_3x";
                    PostgreSQLCompositeType y = db.CompositeTypes.Single(x => x.Name == "tp_u_3");
                    y.Attributes.Single().DataType.Name = "BIGINT";
                }, caseName, caseDescription);
                break;
            case "t_u_4":
                TestCase(db =>
                {
                    Column x = db.Tables.Single(x => x.Name == "t_u_4").Columns.Single(x => x.Name == "c1");
                    x.Name = "c1x";
                }, caseName, caseDescription);
                break;
            case "t_u_4-2":
                TestCase(db =>
                {
                    Column x = db.Tables.Single(x => x.Name == "t_u_4").Columns.Single(x => x.Name == "c1");
                    x.Name = "c1x";
                    PostgreSQLEnumType y = db.EnumTypes.Single(x => x.Name == "tp_u_4");
                    y.AllowedValues = new() { "l1x" };
                }, caseName, caseDescription);
                break;
            case "t_u_5":
                TestCase(db =>
                {
                    Column x = db.Tables.Single(x => x.Name == "t_u_5").Columns.Single(x => x.Name == "c1");
                    x.Name = "c1x";
                    x.Name = "c1x";
                    PostgreSQLCompositeType y = db.CompositeTypes.Single(x => x.Name == "tp_u_5");
                    y.Attributes.Single().DataType.Name = "BIGINT";
                }, caseName, caseDescription);
                break;
            case "t_u_6":
                TestCase(db =>
                {
                    Table x = db.Tables.Single(x => x.Name == "t_u_6");
                    x.Name = "t_u_6x";
                    PostgreSQLCompositeType y = db.CompositeTypes.Single(x => x.Name == "tp_u_6");
                    y.Attributes.Single().DataType.Name = "BIGINT";
                }, caseName, caseDescription);
                break;
            case "v_u_7":
                TestCase(db =>
                {
                    View x = db.Views.Single(x => x.Name == "v_u_7");
                    x.Name = "v_u_7x";
                    x.CreateStatement.Code = x.CreateStatement.Code.Replace("v_u_7", "v_u_7x");
                }, caseName, caseDescription);
                break;
            case "v_u_7-2":
                TestCase(db =>
                {
                    View x = db.Views.Single(x => x.Name == "v_u_7");
                    x.Name = "v_u_7x";
                    x.CreateStatement.Code = x.CreateStatement.Code.Replace("v_u_7", "v_u_7x");
                    PostgreSQLCompositeType y = db.CompositeTypes.Single(x => x.Name == "tp_u_7");
                    y.Attributes.Single().DataType.Name = "BIGINT";
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
