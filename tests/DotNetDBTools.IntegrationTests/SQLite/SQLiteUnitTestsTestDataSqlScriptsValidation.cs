using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Dapper;
using DotNetDBTools.Analysis;
using DotNetDBTools.Analysis.Errors;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Deploy;
using DotNetDBTools.Generation;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.IntegrationTests.Utilities;
using DotNetDBTools.Models.Core;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.Data.Sqlite;
using NUnit.Framework;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.SQLite;

public class SQLiteUnitTestsTestDataSqlScriptsValidation
{
    private static string SpecificDbmsSampleDbV1AssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDB.SQLite.dll";
    private static string SpecificDbmsSampleDbV2AssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDBv2.SQLite.dll";
    private static string UnitTestsTestDataDir => $"{RepoRoot}/tests/DotNetDBTools.UnitTests/TestData";
    private static string DeployOrderTestDataDir => $"{UnitTestsTestDataDir}/SQLite/DeployOrder";
    private const string DbFilesFolder = @"./SQLiteUnitTestsTestDataSqlScriptsValidation_SQLite_DbFiles";
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
            string createScript = new SQLiteDeployManager().GenerateNoDNDBTInfoPublishScript(dbFromDefinition);

            IDbConnection connection = RecreateDbAndCreateConnection(CurrentTestName);
            connection.Execute(createScript);

            Database db = new SQLiteDeployManager().CreateDatabaseModelUsingDBMSSysInfo(connection);
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
            Database db = new SQLiteDeployManager().CreateDatabaseModelUsingDBMSSysInfo(connection);
            return db;
        }
    }

    [Test]
    public void DeployOrderTests_ExpectedUpdateScripts_AreValid()
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
            statements, dbVersion: 0, DatabaseKind.SQLite);
        if (!new AnalysisManager().DbIsValid(reparsedDb, out List<DbError> dbErrors))
            throw new Exception($"Db is invalid:\n{string.Join("\n", dbErrors.Select(x => x.ErrorMessage))}");

        return reparsedDb;
    }

    private IDbConnection RecreateDbAndCreateConnection(string databaseName)
    {
        GC.WaitForPendingFinalizers();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        string shortenedDbName = databaseName.Substring(0, Math.Min(databaseName.Length, 63));
        SQLiteDatabaseHelper.DropDatabaseIfExists(DbFilesFolder, shortenedDbName);
        SQLiteDatabaseHelper.CreateDatabase(DbFilesFolder, shortenedDbName);

        SqliteConnection connection = new();
        connection.ConnectionString = SQLiteDatabaseHelper.CreateConnectionString(DbFilesFolder, shortenedDbName);
        if (GetSqliteAssemblyMajorVersion() >= 6)
            connection.ConnectionString += "Pooling=False;";
        return connection;

        static int GetSqliteAssemblyMajorVersion()
        {
            string sqliteAssemblyMajorVersionStr = typeof(SqliteConnection).Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion
                ?.Split('.')[0];
            int res = int.Parse(sqliteAssemblyMajorVersionStr);
            return res;
        }
    }

    private void AssertDbModelEquivalence(Database dbModel1, Database dbModel2)
    {
        dbModel1.Should().BeEquivalentTo(dbModel2, options => options
            .RespectingRuntimeTypes()
            .WithStrictOrdering()
            .ExcludingDependencies()
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
