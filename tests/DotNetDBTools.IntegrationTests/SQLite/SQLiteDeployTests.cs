using System;
using System.IO;
using DotNetDBTools.Analysis.SQLite;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Deploy;
using DotNetDBTools.Deploy.SQLite;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.SQLite;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.SQLite
{
    [TestClass]
    public class SQLiteDeployTests
    {
        private const string DbFilesFolder = @"./tmp";

        private static readonly string s_agnosticSampleDbAssemblyPath = $"{SamplesOutputDir}/DotNetDBTools.SampleDB.Agnostic.dll";
        private static readonly string s_sqliteSampleDbAssemblyPath = $"{SamplesOutputDir}/DotNetDBTools.SampleDB.SQLite.dll";

        private string ConnectionString => $@"DataSource={DbFilesFolder}/{TestContext.TestName}.db;Mode=ReadWriteCreate;";
        public TestContext TestContext { get; set; }

        private SQLiteDeployManager _deployManager;
        private SqliteConnection _connection;
        private SQLiteInteractor _interactor;

        [TestInitialize]
        public void TestInitialize()
        {
            DropDatabaseIfExists(ConnectionString);

            _deployManager = new(new DeployOptions());
            _connection = new(ConnectionString);
            _interactor = new(new SQLiteQueryExecutor(_connection));
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

            SQLiteDatabase dbModelFromDbAssembly = (SQLiteDatabase)new SQLiteDbModelConverter().FromAgnostic(
                (AgnosticDatabase)DbDefinitionParser.CreateDatabaseModel(s_agnosticSampleDbAssemblyPath));
            SQLiteDatabase dbModelFromDNDBTSysInfo = (SQLiteDatabase)_interactor.GetDatabaseModelFromDNDBTSysInfo();

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

            SQLiteDatabase dbModelFromDbAssembly = (SQLiteDatabase)new SQLiteDbModelConverter().FromAgnostic(
                (AgnosticDatabase)DbDefinitionParser.CreateDatabaseModel(s_agnosticSampleDbAssemblyPath));
            SQLiteDatabase dbModelFromDBMSSysInfo = (SQLiteDatabase)_interactor.GenerateDatabaseModelFromDBMSSysInfo();

            dbModelFromDBMSSysInfo.Should().BeEquivalentTo(dbModelFromDbAssembly, options => options
                .Excluding(database => database.Name)
                .Excluding(database => database.Views)
                .Excluding(database => database.Path.EndsWith(".ID", StringComparison.Ordinal)));
        }

        [TestMethod]
        public void Publish_SQLiteSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            _deployManager.RegisterAsDNDBT(_connection);
            _deployManager.PublishDatabase(s_sqliteSampleDbAssemblyPath, _connection);
            _deployManager.PublishDatabase(s_sqliteSampleDbAssemblyPath, _connection);
        }

        [TestMethod]
        public void SQLiteSampleDB_DbModelFromDNDBTSysInfo_IsEquivalentTo_DbModelFromDbAssembly()
        {
            _deployManager.RegisterAsDNDBT(_connection);
            _deployManager.PublishDatabase(s_sqliteSampleDbAssemblyPath, _connection);

            SQLiteDatabase dbModelFromDbAssembly = (SQLiteDatabase)DbDefinitionParser.CreateDatabaseModel(s_sqliteSampleDbAssemblyPath);
            SQLiteDatabase dbModelFromDNDBTSysInfo = (SQLiteDatabase)_interactor.GetDatabaseModelFromDNDBTSysInfo();

            dbModelFromDNDBTSysInfo.Should().BeEquivalentTo(dbModelFromDbAssembly, options => options
                .Excluding(database => database.Name)
                .Excluding(database => database.Views));
        }

        [TestMethod]
        public void SQLiteSampleDB_DbModelFromDBMSSysInfo_IsEquivalentTo_DbModelFromDbAssembly()
        {
            _deployManager.RegisterAsDNDBT(_connection);
            _deployManager.PublishDatabase(s_sqliteSampleDbAssemblyPath, _connection);
            _deployManager.UnregisterAsDNDBT(_connection);

            SQLiteDatabase dbModelFromDbAssembly = (SQLiteDatabase)DbDefinitionParser.CreateDatabaseModel(s_sqliteSampleDbAssemblyPath);
            SQLiteDatabase dbModelFromDBMSSysInfo = (SQLiteDatabase)_interactor.GenerateDatabaseModelFromDBMSSysInfo();

            dbModelFromDBMSSysInfo.Should().BeEquivalentTo(dbModelFromDbAssembly, options => options
                .Excluding(database => database.Name)
                .Excluding(database => database.Views)
                .Excluding(database => database.Path.EndsWith(".ID", StringComparison.Ordinal)));
        }

        private static void DropDatabaseIfExists(string connectionString)
        {
            SqliteConnectionStringBuilder sqlConnectionBuilder = new(connectionString);
            string dbFilePath = sqlConnectionBuilder.DataSource;

            if (File.Exists(dbFilePath))
                File.Delete(dbFilePath);
            Directory.CreateDirectory(Path.GetDirectoryName(dbFilePath));
        }
    }
}
