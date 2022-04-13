using System;
using System.Collections.Generic;
using System.Data.Common;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Deploy;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using FluentAssertions;
using FluentAssertions.Equivalency;
using NUnit.Framework;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.Base;

[TestFixture]
public abstract class BaseDeployTests<TDatabase, TDbConnection, TDbModelConverter, TDeployManager>
    where TDatabase : Database
    where TDbConnection : DbConnection, new()
    where TDbModelConverter : IDbModelConverter, new()
    where TDeployManager : IDeployManager, new()
{
    protected abstract string SpecificDbmsSampleDbV1AssemblyPath { get; }
    protected abstract string SpecificDbmsSampleDbV2AssemblyPath { get; }
    protected abstract BaseDataTester DataTester { get; set; }

    private static string AgnosticSampleDbV1AssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDB.Agnostic.dll";
    private static string AgnosticSampleDbV2AssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDBv2.Agnostic.dll";
    private static string CurrentTestName => TestContext.CurrentContext.Test.Name;

    private readonly TDeployManager _deployManager;
    private readonly IDbModelFromDefinitionProvider _dbModelFromDefinitionProvider;

    protected BaseDeployTests()
    {
        _deployManager = new();
        _dbModelFromDefinitionProvider = new GenericDbModelFromDefinitionProvider();
    }

    [Test]
    public void RegisterAsDNDBT_PopulatesDNDBTSysTablesCorrectly()
    {
        TestCase(AgnosticSampleDbV1AssemblyPath, "AgnosticV1");
        TestCase(AgnosticSampleDbV2AssemblyPath, "AgnosticV2");
        TestCase(SpecificDbmsSampleDbV1AssemblyPath, "SpecificDbmsV1");
        TestCase(SpecificDbmsSampleDbV2AssemblyPath, "SpecificDbmsV2");

        void TestCase(string sampleDbAssemblyPath, string testCaseId)
        {
            using DbConnection connection = RecreateDbAndCreateConnection(CreateTestCaseName(testCaseId));

            _deployManager.RegisterAsDNDBT(connection);
            _deployManager.PublishDatabase(sampleDbAssemblyPath, connection);

            _deployManager.UnregisterAsDNDBT(connection);
            _deployManager.RegisterAsDNDBT(connection, sampleDbAssemblyPath);

            TDatabase dbModelFromDefinition = CreateSpecificDbmsDbModelFromDefinition(sampleDbAssemblyPath);
            TDatabase dbModelFromDBMSUsingDNDBTSysInfo = (TDatabase)_deployManager.CreateDatabaseModelUsingDNDBTSysInfo(connection);

            AssertDbModelEquivalence(dbModelFromDefinition, dbModelFromDBMSUsingDNDBTSysInfo, CompareMode.None);
        }
    }

    [Test]
    public void Publish_CreatesDb_And_UpdatesItAgain_WithoutErrors()
    {
        TestCase(AgnosticSampleDbV1AssemblyPath, "AgnosticV1");
        TestCase(AgnosticSampleDbV2AssemblyPath, "AgnosticV2");
        TestCase(SpecificDbmsSampleDbV1AssemblyPath, "SpecificDbmsV1");
        TestCase(SpecificDbmsSampleDbV2AssemblyPath, "SpecificDbmsV2");

        void TestCase(string sampleDbAssemblyPath, string testCaseId)
        {
            using DbConnection connection = RecreateDbAndCreateConnection(CreateTestCaseName(testCaseId));

            _deployManager.RegisterAsDNDBT(connection);
            _deployManager.PublishDatabase(sampleDbAssemblyPath, connection);
            _deployManager.PublishDatabase(sampleDbAssemblyPath, connection);
        }
    }

    [Test]
    public void DbModelFromDNDBTSysInfo_IsEquivalentTo_DbModelFromDefinition()
    {
        TestCase(AgnosticSampleDbV1AssemblyPath, "AgnosticV1");
        TestCase(AgnosticSampleDbV2AssemblyPath, "AgnosticV2");
        TestCase(SpecificDbmsSampleDbV1AssemblyPath, "SpecificDbmsV1");
        TestCase(SpecificDbmsSampleDbV2AssemblyPath, "SpecificDbmsV2");

        void TestCase(string sampleDbAssemblyPath, string testCaseId)
        {
            using DbConnection connection = RecreateDbAndCreateConnection(CreateTestCaseName(testCaseId));

            _deployManager.RegisterAsDNDBT(connection);
            _deployManager.PublishDatabase(sampleDbAssemblyPath, connection);

            TDatabase dbModelFromDefinition = CreateSpecificDbmsDbModelFromDefinition(sampleDbAssemblyPath);
            TDatabase dbModelFromDBMSUsingDNDBTSysInfo = (TDatabase)_deployManager.CreateDatabaseModelUsingDNDBTSysInfo(connection);

            AssertDbModelEquivalence(dbModelFromDefinition, dbModelFromDBMSUsingDNDBTSysInfo, CompareMode.None);
        }
    }

    [Test]
    public void DbModelFromDBMSSysInfo_IsEquivalentTo_DbModelFromDefinition()
    {
        TestCase(AgnosticSampleDbV1AssemblyPath, "AgnosticV1");
        TestCase(AgnosticSampleDbV2AssemblyPath, "AgnosticV2");
        TestCase(SpecificDbmsSampleDbV1AssemblyPath, "SpecificDbmsV1");
        TestCase(SpecificDbmsSampleDbV2AssemblyPath, "SpecificDbmsV2");

        void TestCase(string sampleDbAssemblyPath, string testCaseId)
        {
            using DbConnection connection = RecreateDbAndCreateConnection(CreateTestCaseName(testCaseId));

            _deployManager.RegisterAsDNDBT(connection);
            _deployManager.PublishDatabase(sampleDbAssemblyPath, connection);
            _deployManager.UnregisterAsDNDBT(connection);

            TDatabase dbModelFromDefinition = CreateSpecificDbmsDbModelFromDefinition(sampleDbAssemblyPath);
            TDatabase dbModelFromDBMSUsingDBMSSysInfo = (TDatabase)_deployManager.CreateDatabaseModelUsingDBMSSysInfo(connection);

            AssertDbModelEquivalence(dbModelFromDefinition, dbModelFromDBMSUsingDBMSSysInfo, CompareMode.IgnoreAllDNDBTSysInfoSpecific);
        }
    }

    [Test]
    public void DbModelFromDBMSSysInfo_IsValid_And_CorrectlyRepresentsDatabase()
    {
        TestCase(AgnosticSampleDbV1AssemblyPath, "AgnosticV1");
        TestCase(AgnosticSampleDbV2AssemblyPath, "AgnosticV2");
        TestCase(SpecificDbmsSampleDbV1AssemblyPath, "SpecificDbmsV1");
        TestCase(SpecificDbmsSampleDbV2AssemblyPath, "SpecificDbmsV2");

        void TestCase(string sampleDbAssemblyPath, string testCaseId)
        {
            using DbConnection connection1 = RecreateDbAndCreateConnection($"Db1_{CreateTestCaseName(testCaseId)}");
            using DbConnection connection2 = RecreateDbAndCreateConnection($"Db2_{CreateTestCaseName(testCaseId)}");

            _deployManager.RegisterAsDNDBT(connection1);
            _deployManager.PublishDatabase(sampleDbAssemblyPath, connection1);
            TDatabase dbModel1 = (TDatabase)_deployManager.CreateDatabaseModelUsingDBMSSysInfo(connection1);
            dbModel1.Version = 1;

            _deployManager.RegisterAsDNDBT(connection2);
            _deployManager.PublishDatabase(dbModel1, connection2);
            TDatabase dbModel2 = (TDatabase)_deployManager.CreateDatabaseModelUsingDBMSSysInfo(connection2);
            dbModel2.Version = 1;

            AssertDbModelEquivalence(dbModel1, dbModel2, CompareMode.IgnoreIDs);
        }
    }

    [Test]
    public void Publish_Preserves_RelevantData()
    {
        DataTester.IsDbmsSpecific = false;
        TestCase(AgnosticSampleDbV1AssemblyPath, AgnosticSampleDbV2AssemblyPath, "Agnostic");

        DataTester.IsDbmsSpecific = true;
        TestCase(SpecificDbmsSampleDbV1AssemblyPath, SpecificDbmsSampleDbV2AssemblyPath, "SpecificDbms");

        void TestCase(string assemblyV1Path, string assemblyV2Path, string testCaseId)
        {
            using DbConnection connection = RecreateDbAndCreateConnection(CreateTestCaseName(testCaseId));

            _deployManager.RegisterAsDNDBT(connection);
            _deployManager.PublishDatabase(assemblyV1Path, connection);

            DataTester.Populate_SampleDb_WithData(connection);
            DataTester.Assert_SampleDb_Data(connection, BaseDataTester.AssertKind.V1);

            _deployManager.PublishDatabase(assemblyV1Path, connection);
            DataTester.Assert_SampleDb_Data(connection, BaseDataTester.AssertKind.V1);

            _deployManager.Options.AllowDataLoss = true;
            _deployManager.PublishDatabase(assemblyV2Path, connection);
            DataTester.Assert_SampleDb_Data(connection, BaseDataTester.AssertKind.V2);

            _deployManager.Options.AllowDataLoss = false;
            _deployManager.PublishDatabase(assemblyV2Path, connection);
            DataTester.Assert_SampleDb_Data(connection, BaseDataTester.AssertKind.V2);

            _deployManager.Options.AllowDataLoss = true;
            _deployManager.PublishDatabase(assemblyV1Path, connection);
            DataTester.Assert_SampleDb_Data(connection, BaseDataTester.AssertKind.V1Rollbacked);
        }
    }

    private TDatabase CreateSpecificDbmsDbModelFromDefinition(string sampleDbAssemblyPath)
    {
        Database database = _dbModelFromDefinitionProvider.CreateDbModel(
            AssemblyLoader.LoadDbAssemblyFromDll(sampleDbAssemblyPath));

        if (database.Kind == DatabaseKind.Agnostic)
            return (TDatabase)new TDbModelConverter().FromAgnostic(database);
        else
            return (TDatabase)database;
    }

    private void AssertDbModelEquivalence(TDatabase dbModel1, TDatabase dbModel2, CompareMode compareMode)
    {
        dbModel2.Should().BeEquivalentTo(dbModel1, options =>
        {
            EquivalencyAssertionOptions<TDatabase> configuredOptions = options.Excluding(database => database.Name);

            if (compareMode.HasFlag(CompareMode.IgnoreIDs))
                configuredOptions = configuredOptions.Excluding(database => database.Path.EndsWith(".ID", StringComparison.Ordinal));

            if (compareMode.HasFlag(CompareMode.NormalizeCodePieces))
                configuredOptions = configuredOptions.Using(new CodePieceComparer(GetNormalizedCodeFromCodePiece));

            if (compareMode.HasFlag(CompareMode.IgnoreScripts))
                configuredOptions = configuredOptions.Excluding(database => database.Scripts);

            if (compareMode.HasFlag(CompareMode.IgnoreDbAttributes))
                configuredOptions = configuredOptions.Excluding(database => database.Version);

            configuredOptions = AddAdditionalDbModelEquivalenceyOptions(configuredOptions, compareMode);
            return configuredOptions;
        });
    }

    private TDbConnection RecreateDbAndCreateConnection(string testName)
    {
        DropDatabaseIfExists(testName);
        CreateDatabase(testName);

        TDbConnection connection = new();
        connection.ConnectionString = CreateConnectionString(testName);
        return connection;
    }

    private string CreateTestCaseName(string testCaseId)
    {
        return testCaseId + "_" + CurrentTestName;
    }

    protected abstract EquivalencyAssertionOptions<TDatabase> AddAdditionalDbModelEquivalenceyOptions(
        EquivalencyAssertionOptions<TDatabase> options, CompareMode compareMode);
    protected abstract string GetNormalizedCodeFromCodePiece(CodePiece codePiece);
    protected abstract void CreateDatabase(string testName);
    protected abstract void DropDatabaseIfExists(string testName);
    protected abstract string CreateConnectionString(string testName);
    private protected abstract IDbModelFromDBMSProvider CreateDbModelFromDBMSProvider(DbConnection connection);

    protected enum CompareMode
    {
        None = 0,
        IgnoreIDs = 1,
        NormalizeCodePieces = 2,
        IgnoreScripts = 4,
        IgnoreDbAttributes = 8,
        IgnoreAllDNDBTSysInfoSpecific = 1 | 2 | 4 | 8,
    }

    private class CodePieceComparer : IEqualityComparer<CodePiece>
    {
        private readonly Func<CodePiece, string> _getNormalizedCodeFromCodePiece;

        public CodePieceComparer(Func<CodePiece, string> getNormalizedCodeFromCodePiece)
        {
            _getNormalizedCodeFromCodePiece = getNormalizedCodeFromCodePiece;
        }

        public bool Equals(CodePiece x, CodePiece y)
        {
            if (x is null && y is null)
                return true;
            else if (x is null || y is null)
                return false;

            if (x.Code is null && y.Code is null)
                return true;
            else if (x.Code is null || y.Code is null)
                return false;

            string xNormalizedCode = _getNormalizedCodeFromCodePiece(x);
            string yNormalizedCode = _getNormalizedCodeFromCodePiece(y);
            return string.Equals(xNormalizedCode, yNormalizedCode, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(CodePiece obj)
        {
            return obj.Code?.GetHashCode() ?? 0;
        }
    }
}
