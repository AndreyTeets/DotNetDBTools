using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Deploy;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;
using FluentAssertions;
using FluentAssertions.Equivalency;
using NUnit.Framework;

namespace DotNetDBTools.IntegrationTests.Base;

[TestFixture]
public abstract class BaseDeployTests<TDatabase, TDbConnection, TDbModelConverter, TDeployManager>
    where TDatabase : Database
    where TDbConnection : DbConnection, new()
    where TDbModelConverter : IDbModelConverter, new()
    where TDeployManager : IDeployManager, new()
{
    protected abstract string AgnosticSampleDbAssemblyPath { get; }
    protected abstract string SpecificDBMSSampleDbAssemblyPath { get; }
    protected abstract string ActualFilesDir { get; }

    private TDeployManager _deployManager;
    private TDbConnection _connection;
    private IDbModelFromDefinitionProvider _dbModelFromDefinitionProvider;
    private IDbModelFromDBMSProvider _dbModelFromDBMSProvider;

    [SetUp]
    public void SetUp()
    {
        DropDatabaseIfExists(TestContext.CurrentContext.Test.Name);
        CreateDatabase(TestContext.CurrentContext.Test.Name);

        _deployManager = new();
        _connection = new();
        _connection.ConnectionString = CreateConnectionString(TestContext.CurrentContext.Test.Name);
        _dbModelFromDefinitionProvider = new GenericDbModelFromDefinitionProvider();
        _dbModelFromDBMSProvider = CreateDbModelFromDBMSProvider(_connection);
    }

    [TearDown]
    public void TearDown()
    {
        _connection?.Dispose();
    }

    [Test]
    public void RegisterAsDNDBT_FromProvidedDbAssembly_WorksCorrectly()
    {
        _deployManager.RegisterAsDNDBT(_connection);
        _deployManager.PublishDatabase(AgnosticSampleDbAssemblyPath, _connection);

        _deployManager.UnregisterAsDNDBT(_connection);
        _deployManager.RegisterAsDNDBT(_connection, AgnosticSampleDbAssemblyPath);

        string outputPath = @$"{ActualFilesDir}/Actual_PublishScript_For_SampleDB_WhenUpdatingFromV1ToV1.sql";
        _deployManager.GeneratePublishScript(AgnosticSampleDbAssemblyPath, _connection, outputPath);
        string actualScript = File.ReadAllText(outputPath);
        actualScript.Should().Be("");
    }

    [Test]
    public void Publish_AgnosticSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
    {
        _deployManager.RegisterAsDNDBT(_connection);
        _deployManager.PublishDatabase(AgnosticSampleDbAssemblyPath, _connection);
        _deployManager.PublishDatabase(AgnosticSampleDbAssemblyPath, _connection);
    }

    [Test]
    public void AgnosticSampleDB_DbModelFromDBMSUsingDNDBTSysInfo_IsEquivalentTo_DbModelFromDefinition()
    {
        _deployManager.RegisterAsDNDBT(_connection);
        _deployManager.PublishDatabase(AgnosticSampleDbAssemblyPath, _connection);

        TDatabase dbModelFromDefinition = (TDatabase)new TDbModelConverter().FromAgnostic(
            (AgnosticDatabase)_dbModelFromDefinitionProvider.CreateDbModel(
                AssemblyLoader.LoadDbAssemblyFromDll(AgnosticSampleDbAssemblyPath)));
        TDatabase dbModelFromDBMSUsingDNDBTSysInfo = (TDatabase)_dbModelFromDBMSProvider.CreateDbModelUsingDNDBTSysInfo();

        AssertDbModelEquivalence(dbModelFromDefinition, dbModelFromDBMSUsingDNDBTSysInfo, CompareMode.None);
    }

    [Test]
    public void AgnosticSampleDB_DbModelFromDBMSUsingDBMSSysInfo_IsEquivalentTo_DbModelFromDefinition()
    {
        _deployManager.RegisterAsDNDBT(_connection);
        _deployManager.PublishDatabase(AgnosticSampleDbAssemblyPath, _connection);
        _deployManager.UnregisterAsDNDBT(_connection);

        TDatabase dbModelFromDefinition = (TDatabase)new TDbModelConverter().FromAgnostic(
            (AgnosticDatabase)_dbModelFromDefinitionProvider.CreateDbModel(
                AssemblyLoader.LoadDbAssemblyFromDll(AgnosticSampleDbAssemblyPath)));
        TDatabase dbModelFromDBMSUsingDBMSSysInfo = (TDatabase)_dbModelFromDBMSProvider.CreateDbModelUsingDBMSSysInfo();

        AssertDbModelEquivalence(dbModelFromDefinition, dbModelFromDBMSUsingDBMSSysInfo, CompareMode.IgnoreAllDNDBTSysInfoSpecific);
    }

    [Test]
    public void Publish_SpecificDBMSSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
    {
        _deployManager.RegisterAsDNDBT(_connection);
        _deployManager.PublishDatabase(SpecificDBMSSampleDbAssemblyPath, _connection);
        _deployManager.PublishDatabase(SpecificDBMSSampleDbAssemblyPath, _connection);
    }

    [Test]
    public void SpecificDBMSSampleDB_DbModelFromDBMSUsingDNDBTSysInfo_IsEquivalentTo_DbModelFromDefinition()
    {
        _deployManager.RegisterAsDNDBT(_connection);
        _deployManager.PublishDatabase(SpecificDBMSSampleDbAssemblyPath, _connection);

        TDatabase dbModelFromDefinition = (TDatabase)_dbModelFromDefinitionProvider.CreateDbModel(
            AssemblyLoader.LoadDbAssemblyFromDll(SpecificDBMSSampleDbAssemblyPath));
        TDatabase dbModelFromDBMSUsingDNDBTSysInfo = (TDatabase)_dbModelFromDBMSProvider.CreateDbModelUsingDNDBTSysInfo();

        AssertDbModelEquivalence(dbModelFromDefinition, dbModelFromDBMSUsingDNDBTSysInfo, CompareMode.None);
    }

    [Test]
    public void SpecificDBMSSampleDB_DbModelFromDBMSUsingDBMSSysInfo_IsEquivalentTo_DbModelFromDefinition()
    {
        _deployManager.RegisterAsDNDBT(_connection);
        _deployManager.PublishDatabase(SpecificDBMSSampleDbAssemblyPath, _connection);
        _deployManager.UnregisterAsDNDBT(_connection);

        TDatabase dbModelFromDefinition = (TDatabase)_dbModelFromDefinitionProvider.CreateDbModel(
            AssemblyLoader.LoadDbAssemblyFromDll(SpecificDBMSSampleDbAssemblyPath));
        TDatabase dbModelFromDBMSUsingDBMSSysInfo = (TDatabase)_dbModelFromDBMSProvider.CreateDbModelUsingDBMSSysInfo();

        AssertDbModelEquivalence(dbModelFromDefinition, dbModelFromDBMSUsingDBMSSysInfo, CompareMode.IgnoreAllDNDBTSysInfoSpecific);
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

            configuredOptions = AddAdditionalDbModelEquivalenceyOptions(configuredOptions);
            return configuredOptions;
        });
    }

    protected abstract EquivalencyAssertionOptions<TDatabase> AddAdditionalDbModelEquivalenceyOptions(
        EquivalencyAssertionOptions<TDatabase> options);
    protected abstract string GetNormalizedCodeFromCodePiece(CodePiece codePiece);
    protected abstract void CreateDatabase(string testName);
    protected abstract void DropDatabaseIfExists(string testName);
    protected abstract string CreateConnectionString(string testName);
    private protected abstract IDbModelFromDBMSProvider CreateDbModelFromDBMSProvider(DbConnection connection);

    private enum CompareMode
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
