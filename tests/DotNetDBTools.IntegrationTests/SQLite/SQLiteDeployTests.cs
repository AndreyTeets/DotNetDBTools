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

        [TestMethod]
        public void Publish_AgnosticSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            DropDatabaseIfExists(ConnectionString);
            SQLiteDeployManager deployManager = new(new DeployOptions { AllowDbCreation = true });
            deployManager.PublishDatabase(s_agnosticSampleDbAssemblyPath, ConnectionString);
            deployManager.PublishDatabase(s_agnosticSampleDbAssemblyPath, ConnectionString);
        }

        [TestMethod]
        public void AgnosticSampleDB_DbModelFromDNDBTSysInfo_IsEquivalentTo_DbModelFromDbAssembly()
        {
            DropDatabaseIfExists(ConnectionString);
            SQLiteInteractor interactor = new(new SQLiteQueryExecutor(() => new SqliteConnection(ConnectionString)));
            SQLiteDeployManager deployManager = new(new DeployOptions { AllowDbCreation = true });
            deployManager.PublishDatabase(s_agnosticSampleDbAssemblyPath, ConnectionString);

            SQLiteDatabase dbModelFromDbAssembly = (SQLiteDatabase)new SQLiteDbModelConverter().FromAgnostic(
                (AgnosticDatabase)DbDefinitionParser.CreateDatabaseModel(s_agnosticSampleDbAssemblyPath));
            SQLiteDatabase dbModelFromDNDBTSysInfo = (SQLiteDatabase)interactor.GetDatabaseModelFromDNDBTSysInfo();

            dbModelFromDNDBTSysInfo.Should().BeEquivalentTo(dbModelFromDbAssembly, options => options
                .Excluding(database => database.Name)
                .Excluding(database => database.Views));
        }

        [TestMethod]
        public void AgnosticSampleDB_DbModelFromDBMSSysInfo_IsEquivalentTo_DbModelFromDbAssembly()
        {
            DropDatabaseIfExists(ConnectionString);
            SQLiteInteractor interactor = new(new SQLiteQueryExecutor(() => new SqliteConnection(ConnectionString)));
            SQLiteDeployManager deployManager = new(new DeployOptions { AllowDbCreation = true });
            deployManager.PublishDatabase(s_agnosticSampleDbAssemblyPath, ConnectionString);
            deployManager.UnregisterAsDNDBT(ConnectionString);

            SQLiteDatabase dbModelFromDbAssembly = (SQLiteDatabase)new SQLiteDbModelConverter().FromAgnostic(
                (AgnosticDatabase)DbDefinitionParser.CreateDatabaseModel(s_agnosticSampleDbAssemblyPath));
            SQLiteDatabase dbModelFromDBMSSysInfo = (SQLiteDatabase)interactor.GenerateDatabaseModelFromDBMSSysInfo();

            dbModelFromDBMSSysInfo.Should().BeEquivalentTo(dbModelFromDbAssembly, options => options
                .Excluding(database => database.Name)
                .Excluding(database => database.Views)
                .Excluding(database => database.Path.EndsWith(".ID", StringComparison.Ordinal)));
        }

        [TestMethod]
        public void Publish_SQLiteSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            DropDatabaseIfExists(ConnectionString);
            SQLiteDeployManager deployManager = new(new DeployOptions { AllowDbCreation = true });
            deployManager.PublishDatabase(s_sqliteSampleDbAssemblyPath, ConnectionString);
            deployManager.PublishDatabase(s_sqliteSampleDbAssemblyPath, ConnectionString);
        }

        [TestMethod]
        public void SQLiteSampleDB_DbModelFromDNDBTSysInfo_IsEquivalentTo_DbModelFromDbAssembly()
        {
            DropDatabaseIfExists(ConnectionString);
            SQLiteInteractor interactor = new(new SQLiteQueryExecutor(() => new SqliteConnection(ConnectionString)));
            SQLiteDeployManager deployManager = new(new DeployOptions { AllowDbCreation = true });
            deployManager.PublishDatabase(s_sqliteSampleDbAssemblyPath, ConnectionString);

            SQLiteDatabase dbModelFromDbAssembly = (SQLiteDatabase)DbDefinitionParser.CreateDatabaseModel(s_sqliteSampleDbAssemblyPath);
            SQLiteDatabase dbModelFromDNDBTSysInfo = (SQLiteDatabase)interactor.GetDatabaseModelFromDNDBTSysInfo();

            dbModelFromDNDBTSysInfo.Should().BeEquivalentTo(dbModelFromDbAssembly, options => options
                .Excluding(database => database.Name)
                .Excluding(database => database.Views));
        }

        [TestMethod]
        public void SQLiteSampleDB_DbModelFromDBMSSysInfo_IsEquivalentTo_DbModelFromDbAssembly()
        {
            DropDatabaseIfExists(ConnectionString);
            SQLiteInteractor interactor = new(new SQLiteQueryExecutor(() => new SqliteConnection(ConnectionString)));
            SQLiteDeployManager deployManager = new(new DeployOptions { AllowDbCreation = true });
            deployManager.PublishDatabase(s_sqliteSampleDbAssemblyPath, ConnectionString);
            deployManager.UnregisterAsDNDBT(ConnectionString);

            SQLiteDatabase dbModelFromDbAssembly = (SQLiteDatabase)DbDefinitionParser.CreateDatabaseModel(s_sqliteSampleDbAssemblyPath);
            SQLiteDatabase dbModelFromDBMSSysInfo = (SQLiteDatabase)interactor.GenerateDatabaseModelFromDBMSSysInfo();

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
