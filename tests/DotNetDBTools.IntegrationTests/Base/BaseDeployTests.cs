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

namespace DotNetDBTools.IntegrationTests.Base
{
    public abstract class BaseDeployTests<TDatabase, TDbConnection, TDbModelConverter, TDeployManager>
        where TDatabase : Database
        where TDbConnection : DbConnection, new()
        where TDbModelConverter : IDbModelConverter, new()
        where TDeployManager : IDeployManager, new()
    {
        public TestContext TestContext { get; set; }

        protected abstract string AgnosticSampleDbAssemblyPath { get; }
        protected abstract string SpecificDBMSSampleDbAssemblyPath { get; }

        private string ConnectionString => CreateConnectionString(TestContext.TestName);
        private TDeployManager _deployManager;
        private TDbConnection _connection;
        private IDbModelFromDbSysInfoBuilder _dbModelFromDbSysInfoBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            DropDatabaseIfExists(ConnectionString);
            CreateDatabase(ConnectionString);

            _deployManager = new();
            _connection = new();
            _connection.ConnectionString = ConnectionString;
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

            AssertDbModelEquivalence(dbModelFromDbAssembly, dbModelFromDNDBTSysInfo, false);
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

            AssertDbModelEquivalence(dbModelFromDbAssembly, dbModelFromDBMSSysInfo, true);
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

            AssertDbModelEquivalence(dbModelFromDbAssembly, dbModelFromDNDBTSysInfo, false);
        }

        [TestMethod]
        public void SpecificDBMSSampleDB_DbModelFromDBMSSysInfo_IsEquivalentTo_DbModelFromDbAssembly()
        {
            _deployManager.RegisterAsDNDBT(_connection);
            _deployManager.PublishDatabase(SpecificDBMSSampleDbAssemblyPath, _connection);
            _deployManager.UnregisterAsDNDBT(_connection);

            TDatabase dbModelFromDbAssembly = (TDatabase)DbDefinitionParser.CreateDatabaseModel(SpecificDBMSSampleDbAssemblyPath);
            TDatabase dbModelFromDBMSSysInfo = (TDatabase)_dbModelFromDbSysInfoBuilder.GenerateDatabaseModelFromDBMSSysInfo();

            AssertDbModelEquivalence(dbModelFromDbAssembly, dbModelFromDBMSSysInfo, true);
        }

        private void AssertDbModelEquivalence(TDatabase dbModelFromDbAssembly, TDatabase dbModelFromDBMSSysInfo, bool excludeIDs)
        {
            dbModelFromDBMSSysInfo.Should().BeEquivalentTo(dbModelFromDbAssembly, options =>
            {
                EquivalencyAssertionOptions<TDatabase> configuredOptions = options
                    .Excluding(database => database.Name)
                    .Excluding(database => database.Views)
                    .Using(new DefaultValueAsFunctionComparer(NormalizeDefaultValueAsFunctionText));

                if (excludeIDs)
                    configuredOptions = configuredOptions.Excluding(database => database.Path.EndsWith(".ID", StringComparison.Ordinal));

                configuredOptions = AddAdditionalDbModelEquivalenceyOptions(configuredOptions);
                return configuredOptions;
            });
        }

        protected abstract EquivalencyAssertionOptions<TDatabase> AddAdditionalDbModelEquivalenceyOptions(
            EquivalencyAssertionOptions<TDatabase> options);
        protected abstract string NormalizeDefaultValueAsFunctionText(string value);
        protected abstract void CreateDatabase(string connectionString);
        protected abstract void DropDatabaseIfExists(string connectionString);
        protected abstract string CreateConnectionString(string testName);
        private protected abstract IDbModelFromDbSysInfoBuilder CreateDbModelFromDbSysInfoBuilder(DbConnection connection);

        private class DefaultValueAsFunctionComparer : IEqualityComparer<DefaultValueAsFunction>
        {
            private readonly Func<string, string> _normalizeDefaultValueAsFunctionText;

            public DefaultValueAsFunctionComparer(Func<string, string> normalizeDefaultValueAsFunctionText)
            {
                _normalizeDefaultValueAsFunctionText = normalizeDefaultValueAsFunctionText;
            }

            public bool Equals(DefaultValueAsFunction x, DefaultValueAsFunction y)
            {
                string xNormalizedFunctionText = _normalizeDefaultValueAsFunctionText(x.FunctionText);
                string yNormalizedFunctionText = _normalizeDefaultValueAsFunctionText(y.FunctionText);
                return string.Equals(xNormalizedFunctionText, yNormalizedFunctionText, StringComparison.OrdinalIgnoreCase);
            }

            public int GetHashCode(DefaultValueAsFunction obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
