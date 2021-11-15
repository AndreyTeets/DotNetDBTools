using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using DotNetDBTools.Analysis.MSSQL;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Deploy;
using DotNetDBTools.Deploy.MSSQL;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.MSSQL;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.MSSQL
{
    [TestClass]
    public class MSSQLDeployTests
    {
        private static readonly string s_connectionStringWithoutDb = MSSQLContainerHelper.MsSqlContainerConnectionString;

        private static readonly string s_agnosticSampleDbAssemblyPath = $"{SamplesOutputDir}/DotNetDBTools.SampleDB.Agnostic.dll";
        private static readonly string s_mssqlSampleDbAssemblyPath = $"{SamplesOutputDir}/DotNetDBTools.SampleDB.MSSQL.dll";

        private string ConnectionString => CreateConnectionString(s_connectionStringWithoutDb, TestContext.TestName);
        public TestContext TestContext { get; set; }

        private MSSQLDeployManager _deployManager;
        private SqlConnection _connection;
        private MSSQLInteractor _interactor;

        [TestInitialize]
        public void TestInitialize()
        {
            DropDatabaseIfExists(ConnectionString);
            CreateDatabase(ConnectionString);

            _deployManager = new(new DeployOptions());
            _connection = new(ConnectionString);
            _interactor = new(new MSSQLQueryExecutor(_connection));
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

            MSSQLDatabase dbModelFromDbAssembly = (MSSQLDatabase)new MSSQLDbModelConverter().FromAgnostic(
                (AgnosticDatabase)DbDefinitionParser.CreateDatabaseModel(s_agnosticSampleDbAssemblyPath));
            MSSQLDatabase dbModelFromDNDBTSysInfo = (MSSQLDatabase)_interactor.GetDatabaseModelFromDNDBTSysInfo();

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

            MSSQLDatabase dbModelFromDbAssembly = (MSSQLDatabase)new MSSQLDbModelConverter().FromAgnostic(
                (AgnosticDatabase)DbDefinitionParser.CreateDatabaseModel(s_agnosticSampleDbAssemblyPath));
            MSSQLDatabase dbModelFromDBMSSysInfo = (MSSQLDatabase)_interactor.GenerateDatabaseModelFromDBMSSysInfo();

            dbModelFromDBMSSysInfo.Should().BeEquivalentTo(dbModelFromDbAssembly, options => options
                .Excluding(database => database.Name)
                .Excluding(database => database.Views)
                .Excluding(database => database.Path.EndsWith(".ID", StringComparison.Ordinal)));
        }

        [TestMethod]
        public void Publish_MSSQLSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            _deployManager.RegisterAsDNDBT(_connection);
            _deployManager.PublishDatabase(s_mssqlSampleDbAssemblyPath, _connection);
            _deployManager.PublishDatabase(s_mssqlSampleDbAssemblyPath, _connection);
        }

        [TestMethod]
        public void MSSQLSampleDB_DbModelFromDNDBTSysInfo_IsEquivalentTo_DbModelFromDbAssembly()
        {
            _deployManager.RegisterAsDNDBT(_connection);
            _deployManager.PublishDatabase(s_mssqlSampleDbAssemblyPath, _connection);

            MSSQLDatabase dbModelFromDbAssembly = (MSSQLDatabase)DbDefinitionParser.CreateDatabaseModel(s_mssqlSampleDbAssemblyPath);
            MSSQLDatabase dbModelFromDNDBTSysInfo = (MSSQLDatabase)_interactor.GetDatabaseModelFromDNDBTSysInfo();

            dbModelFromDNDBTSysInfo.Should().BeEquivalentTo(dbModelFromDbAssembly, options => options
                .Excluding(database => database.Name)
                .Excluding(database => database.Functions)
                .Excluding(database => database.Views)
                .Using(new MSSQLDefaultValueAsFunctionComparer()));
        }

        [TestMethod]
        public void MSSQLSampleDB_DbModelFromDBMSSysInfo_IsEquivalentTo_DbModelFromDbAssembly()
        {
            _deployManager.RegisterAsDNDBT(_connection);
            _deployManager.PublishDatabase(s_mssqlSampleDbAssemblyPath, _connection);
            _deployManager.UnregisterAsDNDBT(_connection);

            MSSQLDatabase dbModelFromDbAssembly = (MSSQLDatabase)DbDefinitionParser.CreateDatabaseModel(s_mssqlSampleDbAssemblyPath);
            MSSQLDatabase dbModelFromDBMSSysInfo = (MSSQLDatabase)_interactor.GenerateDatabaseModelFromDBMSSysInfo();

            dbModelFromDBMSSysInfo.Should().BeEquivalentTo(dbModelFromDbAssembly, options => options
                .Excluding(database => database.Name)
                .Excluding(database => database.Views)
                .Excluding(database => database.Functions)
                .Excluding(database => database.Path.EndsWith(".ID", StringComparison.Ordinal))
                .Using(new MSSQLDefaultValueAsFunctionComparer()));
        }

        private static void CreateDatabase(string connectionString)
        {
            SqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.InitialCatalog;

            using SqlConnection connection = new(s_connectionStringWithoutDb);
            connection.Execute(
$@"CREATE DATABASE {databaseName};");
        }

        private static void DropDatabaseIfExists(string connectionString)
        {
            SqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.InitialCatalog;

            using SqlConnection connection = new(s_connectionStringWithoutDb);
            connection.Execute(
$@"IF EXISTS (SELECT * FROM [sys].[databases] WHERE [name] = '{databaseName}')
BEGIN
    ALTER DATABASE {databaseName}
    SET OFFLINE WITH ROLLBACK IMMEDIATE;

    ALTER DATABASE {databaseName}
    SET ONLINE;

    DROP DATABASE {databaseName};
END;");
        }

        private static string CreateConnectionString(string connectionStringWithoutDb, string databaseName)
        {
            SqlConnectionStringBuilder connectionStringBuilder = new(connectionStringWithoutDb);
            connectionStringBuilder.InitialCatalog = databaseName;
            string connectionString = connectionStringBuilder.ConnectionString;
            return connectionString;
        }

        private class MSSQLDefaultValueAsFunctionComparer : IEqualityComparer<MSSQLDefaultValueAsFunction>
        {
            public bool Equals(MSSQLDefaultValueAsFunction x, MSSQLDefaultValueAsFunction y)
            {
                if (!char.IsLetter(x.FunctionText.FirstOrDefault()) || !char.IsLetter(y.FunctionText.FirstOrDefault()))
                    return string.Equals(x.FunctionText, y.FunctionText, StringComparison.Ordinal);
                string xNormalizedFunctionText = x.FunctionText.Replace("(", "").Replace(")", "");
                string yNormalizedFunctionText = y.FunctionText.Replace("(", "").Replace(")", "");
                return string.Equals(xNormalizedFunctionText, yNormalizedFunctionText, StringComparison.OrdinalIgnoreCase);
            }

            public int GetHashCode(MSSQLDefaultValueAsFunction obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
