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
    public void DbModelFromDBMS_IsEquivalentTo_DbModelFromDefinition()
    {
        TestCase(AgnosticSampleDbV1AssemblyPath, "AgnosticV1");
        TestCase(AgnosticSampleDbV2AssemblyPath, "AgnosticV2");
        TestCase(SpecificDbmsSampleDbV1AssemblyPath, "SpecificDbmsV1");
        TestCase(SpecificDbmsSampleDbV2AssemblyPath, "SpecificDbmsV2");

        void TestCase(string sampleDbAssemblyPath, string testCaseId)
        {
            using DbConnection connection = RecreateDbAndCreateConnection(CreateTestCaseName(testCaseId));

            TDatabase dbModelFromDefinition = CreateDbModelFromDefinition(sampleDbAssemblyPath);

            _deployManager.RegisterAsDNDBT(connection);
            _deployManager.PublishDatabase(sampleDbAssemblyPath, connection);
            AssertDbModelEquivalence(CreateDbModelFromDNDBTSysInfo(connection), dbModelFromDefinition, CompareMode.Everything);
            AssertDbModelEquivalence(CreateDbModelFromDBMSSysInfo(connection), dbModelFromDefinition, CompareMode.IgnoreAllDNDBTSysInfoSpecific);

            _deployManager.UnregisterAsDNDBT(connection);
            AssertDbModelEquivalence(CreateDbModelFromDBMSSysInfo(connection), dbModelFromDefinition, CompareMode.IgnoreAllDNDBTSysInfoSpecific);

            _deployManager.RegisterAsDNDBT(connection);
            AssertDbModelEquivalence(CreateDbModelFromDNDBTSysInfo(connection), dbModelFromDefinition, CompareMode.IgnoreAllDNDBTSysInfoSpecific);
            AssertDbModelEquivalence(CreateDbModelFromDNDBTSysInfo(connection), dbModelFromDefinition, CompareMode.IgnoreAllDNDBTSysInfoSpecific);

            _deployManager.UnregisterAsDNDBT(connection);
            _deployManager.RegisterAsDNDBT(connection, sampleDbAssemblyPath);
            AssertDbModelEquivalence(CreateDbModelFromDNDBTSysInfo(connection), dbModelFromDefinition, CompareMode.Everything);
            AssertDbModelEquivalence(CreateDbModelFromDBMSSysInfo(connection), dbModelFromDefinition, CompareMode.IgnoreAllDNDBTSysInfoSpecific);
        }
    }

    [Test]
    public void DbModelFromDBMSSysInfo_FullyRepresentsDb()
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
    public void Publish_UpdatesToDesiredState_And_PreservesRelevantData()
    {
        DataTester.IsDbmsSpecific = false;
        TestCase(AgnosticSampleDbV1AssemblyPath, AgnosticSampleDbV2AssemblyPath, "Agnostic");

        DataTester.IsDbmsSpecific = true;
        TestCase(SpecificDbmsSampleDbV1AssemblyPath, SpecificDbmsSampleDbV2AssemblyPath, "SpecificDbms");

        void TestCase(string assemblyV1Path, string assemblyV2Path, string testCaseId)
        {
            using DbConnection connection = RecreateDbAndCreateConnection(CreateTestCaseName(testCaseId));

            TDatabase dbModelFromDefinitionV1 = CreateDbModelFromDefinition(assemblyV1Path);
            TDatabase dbModelFromDefinitionV2 = CreateDbModelFromDefinition(assemblyV2Path);

            _deployManager.RegisterAsDNDBT(connection);
            _deployManager.PublishDatabase(assemblyV1Path, connection);
            AssertDbModelEquivalence(CreateDbModelFromDNDBTSysInfo(connection), dbModelFromDefinitionV1, CompareMode.Everything);
            DataTester.Populate_SampleDb_WithData(connection);
            DataTester.Assert_SampleDb_Data(connection, BaseDataTester.AssertKind.V1);

            _deployManager.PublishDatabase(assemblyV1Path, connection);
            AssertDbModelEquivalence(CreateDbModelFromDNDBTSysInfo(connection), dbModelFromDefinitionV1, CompareMode.Everything);
            DataTester.Assert_SampleDb_Data(connection, BaseDataTester.AssertKind.V1);

            _deployManager.Options.AllowDataLoss = true;
            _deployManager.PublishDatabase(assemblyV2Path, connection);
            AssertDbModelEquivalence(CreateDbModelFromDNDBTSysInfo(connection), dbModelFromDefinitionV2, CompareMode.Everything);
            DataTester.Assert_SampleDb_Data(connection, BaseDataTester.AssertKind.V2);

            _deployManager.Options.AllowDataLoss = false;
            _deployManager.PublishDatabase(assemblyV2Path, connection);
            AssertDbModelEquivalence(CreateDbModelFromDNDBTSysInfo(connection), dbModelFromDefinitionV2, CompareMode.Everything);
            DataTester.Assert_SampleDb_Data(connection, BaseDataTester.AssertKind.V2);

            _deployManager.Options.AllowDataLoss = true;
            _deployManager.PublishDatabase(assemblyV1Path, connection);
            AssertDbModelEquivalence(CreateDbModelFromDNDBTSysInfo(connection), dbModelFromDefinitionV1, CompareMode.Everything);
            DataTester.Assert_SampleDb_Data(connection, BaseDataTester.AssertKind.V1Rollbacked);
        }
    }

    private TDatabase CreateDbModelFromDefinition(string sampleDbAssemblyPath)
    {
        Database database = _dbModelFromDefinitionProvider.CreateDbModel(
            AssemblyLoader.LoadDbAssemblyFromDll(sampleDbAssemblyPath));

        if (database.Kind == DatabaseKind.Agnostic)
            return (TDatabase)new TDbModelConverter().FromAgnostic(database);
        else
            return (TDatabase)database;
    }

    private TDatabase CreateDbModelFromDBMSSysInfo(DbConnection connection)
    {
        return (TDatabase)_deployManager.CreateDatabaseModelUsingDBMSSysInfo(connection);
    }

    private TDatabase CreateDbModelFromDNDBTSysInfo(DbConnection connection)
    {
        return (TDatabase)_deployManager.CreateDatabaseModelUsingDNDBTSysInfo(connection);
    }

    private void AssertDbModelEquivalence(TDatabase dbModel1, TDatabase dbModel2, CompareMode compareMode)
    {
        dbModel1.Should().BeEquivalentTo(dbModel2, options =>
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
        Everything = 0,
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
