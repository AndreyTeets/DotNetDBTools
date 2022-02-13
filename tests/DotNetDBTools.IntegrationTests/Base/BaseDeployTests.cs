using System;
using System.Collections.Generic;
using System.Data.Common;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Deploy;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetDBTools.IntegrationTests.Base;

public abstract class BaseDeployTests<TDatabase, TDbConnection, TDbModelConverter, TDeployManager>
    where TDatabase : Database
    where TDbConnection : DbConnection, new()
    where TDbModelConverter : IDbModelConverter, new()
    where TDeployManager : IDeployManager, new()
{
    public TestContext TestContext { get; set; }

    protected abstract string AgnosticSampleDbAssemblyPath { get; }
    protected abstract string SpecificDBMSSampleDbAssemblyPath { get; }

    private TDeployManager _deployManager;
    private TDbConnection _connection;
    private IDbModelFromDbSysInfoBuilder _dbModelFromDbSysInfoBuilder;

    [TestInitialize]
    public void TestInitialize()
    {
        DropDatabaseIfExists(TestContext.TestName);
        CreateDatabase(TestContext.TestName);

        _deployManager = new();
        _connection = new();
        _connection.ConnectionString = CreateConnectionString(TestContext.TestName);
        _dbModelFromDbSysInfoBuilder = CreateDbModelFromDbSysInfoBuilder(_connection);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        _connection?.Dispose();
    }

    [TestMethod]
    public void Publish_AgnosticSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
    {
        _deployManager.RegisterAsDNDBT(_connection);
        _deployManager.PublishDatabase(AgnosticSampleDbAssemblyPath, _connection);
        _deployManager.PublishDatabase(AgnosticSampleDbAssemblyPath, _connection);
    }

    [TestMethod]
    public void AgnosticSampleDB_DbModelFromDNDBTSysInfo_IsEquivalentTo_DbModelFromDbAssembly()
    {
        _deployManager.RegisterAsDNDBT(_connection);
        _deployManager.PublishDatabase(AgnosticSampleDbAssemblyPath, _connection);

        TDatabase dbModelFromDbAssembly = (TDatabase)new TDbModelConverter().FromAgnostic(
            (AgnosticDatabase)DbDefinitionParser.CreateDatabaseModel(AgnosticSampleDbAssemblyPath));
        TDatabase dbModelFromDNDBTSysInfo = (TDatabase)_dbModelFromDbSysInfoBuilder.GetDatabaseModelFromDNDBTSysInfo();

        AssertDbModelEquivalence(dbModelFromDbAssembly, dbModelFromDNDBTSysInfo, CompareMode.None);
    }

    [TestMethod]
    public void AgnosticSampleDB_DbModelFromDBMSSysInfo_IsEquivalentTo_DbModelFromDbAssembly()
    {
        _deployManager.RegisterAsDNDBT(_connection);
        _deployManager.PublishDatabase(AgnosticSampleDbAssemblyPath, _connection);
        _deployManager.UnregisterAsDNDBT(_connection);

        TDatabase dbModelFromDbAssembly = (TDatabase)new TDbModelConverter().FromAgnostic(
            (AgnosticDatabase)DbDefinitionParser.CreateDatabaseModel(AgnosticSampleDbAssemblyPath));
        TDatabase dbModelFromDBMSSysInfo = (TDatabase)_dbModelFromDbSysInfoBuilder.GenerateDatabaseModelFromDBMSSysInfo();

        AssertDbModelEquivalence(dbModelFromDbAssembly, dbModelFromDBMSSysInfo, CompareMode.IgnoreIDsAndNormalizeCodePieces);
    }

    [TestMethod]
    public void Publish_SpecificDBMSSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
    {
        _deployManager.RegisterAsDNDBT(_connection);
        _deployManager.PublishDatabase(SpecificDBMSSampleDbAssemblyPath, _connection);
        _deployManager.PublishDatabase(SpecificDBMSSampleDbAssemblyPath, _connection);
    }

    [TestMethod]
    public void SpecificDBMSSampleDB_DbModelFromDNDBTSysInfo_IsEquivalentTo_DbModelFromDbAssembly()
    {
        _deployManager.RegisterAsDNDBT(_connection);
        _deployManager.PublishDatabase(SpecificDBMSSampleDbAssemblyPath, _connection);

        TDatabase dbModelFromDbAssembly = (TDatabase)DbDefinitionParser.CreateDatabaseModel(SpecificDBMSSampleDbAssemblyPath);
        TDatabase dbModelFromDNDBTSysInfo = (TDatabase)_dbModelFromDbSysInfoBuilder.GetDatabaseModelFromDNDBTSysInfo();

        AssertDbModelEquivalence(dbModelFromDbAssembly, dbModelFromDNDBTSysInfo, CompareMode.None);
    }

    [TestMethod]
    public void SpecificDBMSSampleDB_DbModelFromDBMSSysInfo_IsEquivalentTo_DbModelFromDbAssembly()
    {
        _deployManager.RegisterAsDNDBT(_connection);
        _deployManager.PublishDatabase(SpecificDBMSSampleDbAssemblyPath, _connection);
        _deployManager.UnregisterAsDNDBT(_connection);

        TDatabase dbModelFromDbAssembly = (TDatabase)DbDefinitionParser.CreateDatabaseModel(SpecificDBMSSampleDbAssemblyPath);
        TDatabase dbModelFromDBMSSysInfo = (TDatabase)_dbModelFromDbSysInfoBuilder.GenerateDatabaseModelFromDBMSSysInfo();

        AssertDbModelEquivalence(dbModelFromDbAssembly, dbModelFromDBMSSysInfo, CompareMode.IgnoreIDsAndNormalizeCodePieces);
    }

    private void AssertDbModelEquivalence(TDatabase dbModelFromDbAssembly, TDatabase dbModelFromDBMSSysInfo, CompareMode compareMode)
    {
        dbModelFromDBMSSysInfo.Should().BeEquivalentTo(dbModelFromDbAssembly, options =>
        {
            EquivalencyAssertionOptions<TDatabase> configuredOptions = options.Excluding(database => database.Name);

            if (compareMode.HasFlag(CompareMode.IgnoreIDs))
                configuredOptions = configuredOptions.Excluding(database => database.Path.EndsWith(".ID", StringComparison.Ordinal));

            if (compareMode.HasFlag(CompareMode.NormalizeCodePieces))
                configuredOptions = configuredOptions.Using(new CodePieceComparer(GetNormalizedCodeFromCodePiece));

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
    private protected abstract IDbModelFromDbSysInfoBuilder CreateDbModelFromDbSysInfoBuilder(DbConnection connection);

    private enum CompareMode
    {
        None = 0,
        IgnoreIDs = 1,
        NormalizeCodePieces = 2,
        IgnoreIDsAndNormalizeCodePieces = 1 | 2,
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
