using System;
using System.Collections.Generic;
using Dapper;
using DotNetDBTools.Analysis.PostgreSQL;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Deploy;
using DotNetDBTools.Deploy.PostgreSQL;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.PostgreSQL
{
    [TestClass]
    public class PostgreSQLDeployTests
    {
        private static readonly string s_connectionStringWithoutDb = PostgreSQLContainerHelper.PostgreSQLContainerConnectionString;

        private static readonly string s_agnosticSampleDbAssemblyPath = $"{SamplesOutputDir}/DotNetDBTools.SampleDB.Agnostic.dll";
        private static readonly string s_postgresqlSampleDbAssemblyPath = $"{SamplesOutputDir}/DotNetDBTools.SampleDB.PostgreSQL.dll";

        private string ConnectionString => CreateConnectionString(s_connectionStringWithoutDb, TestContext.TestName);
        public TestContext TestContext { get; set; }

        private PostgreSQLDeployManager _deployManager;
        private NpgsqlConnection _connection;
        private PostgreSQLInteractor _interactor;

        [TestInitialize]
        public void TestInitialize()
        {
            DropDatabaseIfExists(ConnectionString);
            CreateDatabase(ConnectionString);

            _deployManager = new(new DeployOptions());
            _connection = new(ConnectionString);
            _interactor = new(new PostgreSQLQueryExecutor(_connection));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _connection.Dispose();
        }

        [TestMethod]
        public void Publish_AgnosticSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            _deployManager.RegisterAsDNDBT(_connection);
            _deployManager.PublishDatabase(s_agnosticSampleDbAssemblyPath, _connection);
            _deployManager.PublishDatabase(s_agnosticSampleDbAssemblyPath, _connection);
        }

        [TestMethod]
        public void AgnosticSampleDB_DbModelFromDNDBTSysInfo_IsEquivalentTo_DbModelFromDbAssembly()
        {
            _deployManager.RegisterAsDNDBT(_connection);
            _deployManager.PublishDatabase(s_agnosticSampleDbAssemblyPath, _connection);

            PostgreSQLDatabase dbModelFromDbAssembly = (PostgreSQLDatabase)new PostgreSQLDbModelConverter().FromAgnostic(
                (AgnosticDatabase)DbDefinitionParser.CreateDatabaseModel(s_agnosticSampleDbAssemblyPath));
            PostgreSQLDatabase dbModelFromDNDBTSysInfo = (PostgreSQLDatabase)_interactor.GetDatabaseModelFromDNDBTSysInfo();

            dbModelFromDNDBTSysInfo.Should().BeEquivalentTo(dbModelFromDbAssembly, options => options
                .Excluding(database => database.Name)
                .Excluding(database => database.Views));
        }

        [TestMethod]
        public void AgnosticSampleDB_DbModelFromDBMSSysInfo_IsEquivalentTo_DbModelFromDbAssembly()
        {
            _deployManager.RegisterAsDNDBT(_connection);
            _deployManager.PublishDatabase(s_agnosticSampleDbAssemblyPath, _connection);
            _deployManager.UnregisterAsDNDBT(_connection);

            PostgreSQLDatabase dbModelFromDbAssembly = (PostgreSQLDatabase)new PostgreSQLDbModelConverter().FromAgnostic(
                (AgnosticDatabase)DbDefinitionParser.CreateDatabaseModel(s_agnosticSampleDbAssemblyPath));
            PostgreSQLDatabase dbModelFromDBMSSysInfo = (PostgreSQLDatabase)_interactor.GenerateDatabaseModelFromDBMSSysInfo();

            dbModelFromDBMSSysInfo.Should().BeEquivalentTo(dbModelFromDbAssembly, options => options
                .Excluding(database => database.Name)
                .Excluding(database => database.Views)
                .Excluding(database => database.Path.EndsWith(".ID", StringComparison.Ordinal))
                .Using(new DefaultValueAsFunctionComparer()));
        }

        [TestMethod]
        public void Publish_PostgreSQLSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            _deployManager.RegisterAsDNDBT(_connection);
            _deployManager.PublishDatabase(s_postgresqlSampleDbAssemblyPath, _connection);
            _deployManager.PublishDatabase(s_postgresqlSampleDbAssemblyPath, _connection);
        }

        [TestMethod]
        public void PostgreSQLSampleDB_DbModelFromDNDBTSysInfo_IsEquivalentTo_DbModelFromDbAssembly()
        {
            _deployManager.RegisterAsDNDBT(_connection);
            _deployManager.PublishDatabase(s_postgresqlSampleDbAssemblyPath, _connection);

            PostgreSQLDatabase dbModelFromDbAssembly = (PostgreSQLDatabase)DbDefinitionParser.CreateDatabaseModel(s_postgresqlSampleDbAssemblyPath);
            PostgreSQLDatabase dbModelFromDNDBTSysInfo = (PostgreSQLDatabase)_interactor.GetDatabaseModelFromDNDBTSysInfo();

            dbModelFromDNDBTSysInfo.Should().BeEquivalentTo(dbModelFromDbAssembly, options => options
                .Excluding(database => database.Name)
                .Excluding(database => database.Functions)
                .Excluding(database => database.Views));
        }

        [TestMethod]
        public void PostgreSQLSampleDB_DbModelFromDBMSSysInfo_IsEquivalentTo_DbModelFromDbAssembly()
        {
            _deployManager.RegisterAsDNDBT(_connection);
            _deployManager.PublishDatabase(s_postgresqlSampleDbAssemblyPath, _connection);
            _deployManager.UnregisterAsDNDBT(_connection);

            PostgreSQLDatabase dbModelFromDbAssembly = (PostgreSQLDatabase)DbDefinitionParser.CreateDatabaseModel(s_postgresqlSampleDbAssemblyPath);
            PostgreSQLDatabase dbModelFromDBMSSysInfo = (PostgreSQLDatabase)_interactor.GenerateDatabaseModelFromDBMSSysInfo();

            dbModelFromDBMSSysInfo.Should().BeEquivalentTo(dbModelFromDbAssembly, options => options
                .Excluding(database => database.Name)
                .Excluding(database => database.Views)
                .Excluding(database => database.Functions)
                .Excluding(database => database.Path.EndsWith(".ID", StringComparison.Ordinal))
                .Using(new DefaultValueAsFunctionComparer()));
        }

        private static void CreateDatabase(string connectionString)
        {
            NpgsqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.Database;

            using NpgsqlConnection connection = new(s_connectionStringWithoutDb);
            connection.Execute(
$@"CREATE DATABASE ""{databaseName}"";");
        }

        private static void DropDatabaseIfExists(string connectionString)
        {
            NpgsqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.Database;

            using NpgsqlConnection connection = new(s_connectionStringWithoutDb);
            connection.Execute(
$@"SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = '{databaseName}';
DROP DATABASE IF EXISTS ""{databaseName}"";");
        }

        private static string CreateConnectionString(string connectionStringWithoutDb, string databaseName)
        {
            NpgsqlConnectionStringBuilder connectionStringBuilder = new(connectionStringWithoutDb);
            connectionStringBuilder.Database = databaseName;
            string connectionString = connectionStringBuilder.ConnectionString;
            return connectionString;
        }

        private class DefaultValueAsFunctionComparer : IEqualityComparer<DefaultValueAsFunction>
        {
            public bool Equals(DefaultValueAsFunction x, DefaultValueAsFunction y)
            {
                string xNormalizedFunctionText = NormalizeFunctionText(x.FunctionText);
                string yNormalizedFunctionText = NormalizeFunctionText(y.FunctionText);
                return string.Equals(xNormalizedFunctionText, yNormalizedFunctionText, StringComparison.OrdinalIgnoreCase);
            }

            public int GetHashCode(DefaultValueAsFunction obj)
            {
                return obj.GetHashCode();
            }

            private static string NormalizeFunctionText(string value)
            {
                return value.ToUpper()
                    .Replace("::INTEGER", "")
                    .Replace("'", "");
            }
        }
    }
}
