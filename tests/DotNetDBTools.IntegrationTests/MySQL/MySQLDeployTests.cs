using System;
using System.Collections.Generic;
using Dapper;
using DotNetDBTools.Analysis.MySQL;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Deploy;
using DotNetDBTools.Deploy.MySQL;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySqlConnector;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.MySQL
{
    [TestClass]
    public class MySQLDeployTests
    {
        private static readonly string s_connectionStringWithoutDb = MySQLContainerHelper.MySQLContainerConnectionString;

        private static readonly string s_agnosticSampleDbAssemblyPath = $"{SamplesOutputDir}/DotNetDBTools.SampleDB.Agnostic.dll";
        private static readonly string s_mysqlSampleDbAssemblyPath = $"{SamplesOutputDir}/DotNetDBTools.SampleDB.MySQL.dll";

        private string ConnectionString => CreateConnectionString(s_connectionStringWithoutDb, MangleDbNameIfTooLong(TestContext.TestName));
        public TestContext TestContext { get; set; }

        private MySQLDeployManager _deployManager;
        private MySqlConnection _connection;
        private MySQLInteractor _interactor;

        [TestInitialize]
        public void TestInitialize()
        {
            DropDatabaseIfExists(ConnectionString);
            CreateDatabase(ConnectionString);

            _deployManager = new(new DeployOptions());
            _connection = new(ConnectionString);
            _interactor = new(new MySQLQueryExecutor(_connection));
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

            MySQLDatabase dbModelFromDbAssembly = (MySQLDatabase)new MySQLDbModelConverter().FromAgnostic(
                (AgnosticDatabase)DbDefinitionParser.CreateDatabaseModel(s_agnosticSampleDbAssemblyPath));
            MySQLDatabase dbModelFromDNDBTSysInfo = (MySQLDatabase)_interactor.GetDatabaseModelFromDNDBTSysInfo();

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

            MySQLDatabase dbModelFromDbAssembly = (MySQLDatabase)new MySQLDbModelConverter().FromAgnostic(
                (AgnosticDatabase)DbDefinitionParser.CreateDatabaseModel(s_agnosticSampleDbAssemblyPath));
            MySQLDatabase dbModelFromDBMSSysInfo = (MySQLDatabase)_interactor.GenerateDatabaseModelFromDBMSSysInfo();

            dbModelFromDBMSSysInfo.Should().BeEquivalentTo(dbModelFromDbAssembly, options => options
                .Excluding(database => database.Name)
                .Excluding(database => database.Views)
                .Excluding(database => database.Path.EndsWith(".ID", StringComparison.Ordinal))
                .Using(new DefaultValueAsFunctionComparer()));
        }

        [TestMethod]
        public void Publish_MySQLSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            _deployManager.RegisterAsDNDBT(_connection);
            _deployManager.PublishDatabase(s_mysqlSampleDbAssemblyPath, _connection);
            _deployManager.PublishDatabase(s_mysqlSampleDbAssemblyPath, _connection);
        }

        [TestMethod]
        public void MySQLSampleDB_DbModelFromDNDBTSysInfo_IsEquivalentTo_DbModelFromDbAssembly()
        {
            _deployManager.RegisterAsDNDBT(_connection);
            _deployManager.PublishDatabase(s_mysqlSampleDbAssemblyPath, _connection);

            MySQLDatabase dbModelFromDbAssembly = (MySQLDatabase)DbDefinitionParser.CreateDatabaseModel(s_mysqlSampleDbAssemblyPath);
            MySQLDatabase dbModelFromDNDBTSysInfo = (MySQLDatabase)_interactor.GetDatabaseModelFromDNDBTSysInfo();

            dbModelFromDNDBTSysInfo.Should().BeEquivalentTo(dbModelFromDbAssembly, options => options
                .Excluding(database => database.Name)
                .Excluding(database => database.Functions)
                .Excluding(database => database.Views));
        }

        [TestMethod]
        public void MySQLSampleDB_DbModelFromDBMSSysInfo_IsEquivalentTo_DbModelFromDbAssembly()
        {
            _deployManager.RegisterAsDNDBT(_connection);
            _deployManager.PublishDatabase(s_mysqlSampleDbAssemblyPath, _connection);
            _deployManager.UnregisterAsDNDBT(_connection);

            MySQLDatabase dbModelFromDbAssembly = (MySQLDatabase)DbDefinitionParser.CreateDatabaseModel(s_mysqlSampleDbAssemblyPath);
            MySQLDatabase dbModelFromDBMSSysInfo = (MySQLDatabase)_interactor.GenerateDatabaseModelFromDBMSSysInfo();

            dbModelFromDBMSSysInfo.Should().BeEquivalentTo(dbModelFromDbAssembly, options => options
                .Excluding(database => database.Name)
                .Excluding(database => database.Views)
                .Excluding(database => database.Functions)
                .Excluding(database => database.Path.EndsWith(".ID", StringComparison.Ordinal))
                .Using(new DefaultValueAsFunctionComparer()));
        }

        private static void CreateDatabase(string connectionString)
        {
            MySqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.Database;

            using MySqlConnection connection = new(s_connectionStringWithoutDb);
            connection.Execute(
$@"CREATE DATABASE `{databaseName}`;");
        }

        private static void DropDatabaseIfExists(string connectionString)
        {
            MySqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.Database;

            using MySqlConnection connection = new(s_connectionStringWithoutDb);
            connection.Execute(
$@"DROP DATABASE IF EXISTS `{databaseName}`;");
        }

        private static string CreateConnectionString(string connectionStringWithoutDb, string databaseName)
        {
            MySqlConnectionStringBuilder connectionStringBuilder = new(connectionStringWithoutDb);
            connectionStringBuilder.Database = databaseName;
            string connectionString = connectionStringBuilder.ConnectionString;
            return connectionString;
        }

        private static string MangleDbNameIfTooLong(string databaseName)
        {
            if (databaseName.Length > 64)
                return databaseName.Substring(0, 63);
            else
                return databaseName;
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
                    .Replace("(", "")
                    .Replace(")", "");
            }
        }
    }
}
