using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Dapper;
using DotNetDBTools.Analysis;
using DotNetDBTools.Analysis.Errors;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Deploy;
using DotNetDBTools.Generation;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Npgsql;
using NUnit.Framework;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.PostgreSQL;

public class PostgreSQLUnitTestsTestDataSqlScriptsValidation
{
    private static string SpecificDbmsSampleDbV1AssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDB.PostgreSQL.dll";
    private static string SpecificDbmsSampleDbV2AssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDBv2.PostgreSQL.dll";
    private static string UnitTestsTestDataDir => $"{RepoRoot}/tests/DotNetDBTools.UnitTests/TestData";
    private static string DeployOrderTestDataDir => $"{UnitTestsTestDataDir}/PostgreSQL/DeployOrder";
    private static string ConnectionStringWithoutDb => PostgreSQLContainerHelper.PostgreSQLContainerConnectionString;
    private static string CurrentTestName => TestContext.CurrentContext.Test.Name;

    [Test]
    // TODO move this test somewhere else more appropriate
    public void SampleDb_CreateScript_IsValid_AndCreatesValidParsableDatabase()
    {
        TestCase(SpecificDbmsSampleDbV1AssemblyPath);
        TestCase(SpecificDbmsSampleDbV2AssemblyPath);

        void TestCase(string dbAssemblyPath)
        {
            Database dbFromDefinition = new DefinitionParsingManager().CreateDbModel(dbAssemblyPath);
            string createScript = new PostgreSQLDeployManager().GenerateNoDNDBTInfoPublishScript(dbFromDefinition);

            IDbConnection connection = RecreateDbAndCreateConnection(CurrentTestName);
            connection.Execute(createScript);

            Database db = new PostgreSQLDeployManager().CreateDatabaseModelUsingDBMSSysInfo(connection);
            Database reparsedDb = ReparseDatabaseModelFromGeneratedDefinition(db);

            AssertDbModelEquivalence(db, reparsedDb);
        }
    }

    [Test]
    // TODO move this test somewhere else more appropriate
    public void DeployOrderTests_DefinitionScriptAndCreateScript_AreValid_AndCreateValidParsableEquivalentDatabaseModels()
    {
        Database dbFromDefinitionScript = RecreateDb_ExecuteScript_CreateDbModelFromDBMS("DatabaseDefinition.sql");
        Database dbFromCreateScript = RecreateDb_ExecuteScript_CreateDbModelFromDBMS("ExpectedCreateScript.sql");

        AssertDbModelEquivalence(dbFromCreateScript, dbFromDefinitionScript);

        Database reparsedDbFromDefinitionScript = ReparseDatabaseModelFromGeneratedDefinition(dbFromDefinitionScript);
        Database reparsedDbFromCreateScript = ReparseDatabaseModelFromGeneratedDefinition(dbFromCreateScript);

        AssertDbModelEquivalence(reparsedDbFromCreateScript, reparsedDbFromDefinitionScript);
        AssertDbModelEquivalence(reparsedDbFromCreateScript, dbFromCreateScript);

        Database RecreateDb_ExecuteScript_CreateDbModelFromDBMS(string scriptName)
        {
            IDbConnection connection = RecreateDbAndCreateConnection(CurrentTestName);
            connection.Execute(File.ReadAllText($"{DeployOrderTestDataDir}/{scriptName}"));
            Database db = new PostgreSQLDeployManager().CreateDatabaseModelUsingDBMSSysInfo(connection);
            return db;
        }
    }

    [Test]
    public void DeployOrderTests_ExpectedCreateAndUpdateScripts_AreValid()
    {
        string[] updateScriptPaths = Directory.GetFiles(DeployOrderTestDataDir, "ExpectedUpdateScript-*.sql");
        foreach (string updateScriptPath in updateScriptPaths)
        {
            string updateScriptFileName = Path.GetFileName(updateScriptPath);
            TestCase(updateScriptFileName);
        }

        void TestCase(string updateScriptFileName)
        {
            try
            {
                using IDbConnection connection = RecreateDbAndCreateConnection(CurrentTestName);
                connection.Execute(File.ReadAllText($"{DeployOrderTestDataDir}/ExpectedCreateScript.sql"));
                connection.Execute(File.ReadAllText($"{DeployOrderTestDataDir}/{updateScriptFileName}"));
            }
            catch (Exception ex)
            {
                throw new Exception($"Script '{updateScriptFileName}' is invalid.", ex);
            }
        }
    }

    private Database ReparseDatabaseModelFromGeneratedDefinition(Database db)
    {
        List<string> statements = new();
        GenerationManager generator = new(new GenerationOptions() { OutputDefinitionKind = OutputDefinitionKind.Sql });
        foreach (DefinitionSourceFile file in generator.GenerateDefinition(db))
        {
            if (file.RelativePath.EndsWith(".sql", StringComparison.OrdinalIgnoreCase))
                statements.Add(file.SourceText);
        }

        Database reparsedDb = new DefinitionParsingManager().CreateDbModel(
            statements, dbVersion: 0, DatabaseKind.PostgreSQL);
        if (!new AnalysisManager().DbIsValid(reparsedDb, out List<DbError> dbErrors))
            throw new Exception($"Db is invalid:\n{string.Join("\n", dbErrors.Select(x => x.ErrorMessage))}");

        return reparsedDb;
    }

    private IDbConnection RecreateDbAndCreateConnection(string databaseName)
    {
        PostgreSQLDatabaseHelper.DropDatabaseIfExists(ConnectionStringWithoutDb, databaseName);
        PostgreSQLDatabaseHelper.CreateDatabase(ConnectionStringWithoutDb, databaseName);

        NpgsqlConnection.ClearAllPools();
        NpgsqlConnection connection = new();
        connection.ConnectionString = PostgreSQLDatabaseHelper.CreateConnectionString(ConnectionStringWithoutDb, databaseName);
        return connection;
    }

    private void AssertDbModelEquivalence(Database dbModel1, Database dbModel2)
    {
        dbModel1.Should().BeEquivalentTo(dbModel2, options => options
            .RespectingRuntimeTypes()
            .WithStrictOrdering()
            .Excluding(mi => mi.Name == nameof(DbObject.Parent) && mi.DeclaringType == typeof(DbObject))
            .Excluding(mi => mi.Name == nameof(CodePiece.DependsOn) && mi.DeclaringType == typeof(CodePiece))
            .Excluding(mi => mi.Name == nameof(DataType.DependsOn) && mi.DeclaringType == typeof(DataType))
            .Excluding(mi => mi.Name == nameof(DbObject.ID) && mi.DeclaringType == typeof(DbObject))
            .Using<CodePiece>(CompareCodePiece).WhenTypeIs<CodePiece>());
    }

    private void CompareCodePiece(IAssertionContext<CodePiece> ctx)
    {
        if (ctx.Subject is null || ctx.Expectation is null)
        {
            ctx.Subject.Should().Be(ctx.Expectation);
            return;
        }

        string subjectNormalizedCode = ctx.Subject.Code.AppendSemicolonIfAbsent();
        string expectationNormalizedCode = ctx.Expectation.Code.AppendSemicolonIfAbsent();
        subjectNormalizedCode.Should().Be(expectationNormalizedCode);
    }
}
