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
            SQLiteDeployManager deployManager = new(true, false);
            deployManager.PublishDatabase(s_agnosticSampleDbAssemblyPath, ConnectionString);
            deployManager.PublishDatabase(s_agnosticSampleDbAssemblyPath, ConnectionString);
        }

        [TestMethod]
        public void AgnosticSampleDB_DbInfoFromDNDBTSysInfo_IsEquivalentTo_DbInfoFromDbAssembly()
        {
            DropDatabaseIfExists(ConnectionString);
            SQLiteInteractor interactor = new(new SQLiteQueryExecutor(ConnectionString));
            SQLiteDeployManager deployManager = new(true, false);
            deployManager.PublishDatabase(s_agnosticSampleDbAssemblyPath, ConnectionString);

            SQLiteDatabaseInfo dbInfoFromDbAssembly = (SQLiteDatabaseInfo)new SQLiteDbModelConverter().FromAgnostic(
                (AgnosticDatabaseInfo)DbDefinitionParser.CreateDatabaseInfo(s_agnosticSampleDbAssemblyPath));
            SQLiteDatabaseInfo dbInfoFromDNDBTSysInfo = (SQLiteDatabaseInfo)interactor.GetDatabaseModelFromDNDBTSysInfo();

            dbInfoFromDNDBTSysInfo.Should().BeEquivalentTo(dbInfoFromDbAssembly, options => options
                .Excluding(dbInfo => dbInfo.Name)
                .Excluding(dbInfo => dbInfo.Views));
        }

        [TestMethod]
        public void AgnosticSampleDB_DbInfoFromDBMSSysInfo_IsEquivalentTo_DbInfoFromDbAssembly()
        {
            DropDatabaseIfExists(ConnectionString);
            SQLiteInteractor interactor = new(new SQLiteQueryExecutor(ConnectionString));
            SQLiteDeployManager deployManager = new(true, false);
            deployManager.PublishDatabase(s_agnosticSampleDbAssemblyPath, ConnectionString);
            deployManager.UnregisterAsDNDBT(ConnectionString);

            SQLiteDatabaseInfo dbInfoFromDbAssembly = (SQLiteDatabaseInfo)new SQLiteDbModelConverter().FromAgnostic(
                (AgnosticDatabaseInfo)DbDefinitionParser.CreateDatabaseInfo(s_agnosticSampleDbAssemblyPath));
            SQLiteDatabaseInfo dbInfoFromDBMSSysInfo = (SQLiteDatabaseInfo)interactor.GenerateDatabaseModelFromDBMSSysInfo();

            dbInfoFromDBMSSysInfo.Should().BeEquivalentTo(dbInfoFromDbAssembly, options => options
                .Excluding(dbInfo => dbInfo.Name)
                .Excluding(dbInfo => dbInfo.Views)
                .Excluding(dbInfo => dbInfo.Path.EndsWith(".ID", StringComparison.Ordinal)));
        }

        [TestMethod]
        public void Publish_SQLiteSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            DropDatabaseIfExists(ConnectionString);
            SQLiteDeployManager deployManager = new(true, false);
            deployManager.PublishDatabase(s_sqliteSampleDbAssemblyPath, ConnectionString);
            deployManager.PublishDatabase(s_sqliteSampleDbAssemblyPath, ConnectionString);
        }

        [TestMethod]
        public void SQLiteSampleDB_DbInfoFromDNDBTSysInfo_IsEquivalentTo_DbInfoFromDbAssembly()
        {
            DropDatabaseIfExists(ConnectionString);
            SQLiteInteractor interactor = new(new SQLiteQueryExecutor(ConnectionString));
            SQLiteDeployManager deployManager = new(true, false);
            deployManager.PublishDatabase(s_sqliteSampleDbAssemblyPath, ConnectionString);

            SQLiteDatabaseInfo dbInfoFromDbAssembly = (SQLiteDatabaseInfo)DbDefinitionParser.CreateDatabaseInfo(s_sqliteSampleDbAssemblyPath);
            SQLiteDatabaseInfo dbInfoFromDNDBTSysInfo = (SQLiteDatabaseInfo)interactor.GetDatabaseModelFromDNDBTSysInfo();

            dbInfoFromDNDBTSysInfo.Should().BeEquivalentTo(dbInfoFromDbAssembly, options => options
                .Excluding(dbInfo => dbInfo.Name)
                .Excluding(dbInfo => dbInfo.Views));
        }

        [TestMethod]
        public void SQLiteSampleDB_DbInfoFromDBMSSysInfo_IsEquivalentTo_DbInfoFromDbAssembly()
        {
            DropDatabaseIfExists(ConnectionString);
            SQLiteInteractor interactor = new(new SQLiteQueryExecutor(ConnectionString));
            SQLiteDeployManager deployManager = new(true, false);
            deployManager.PublishDatabase(s_sqliteSampleDbAssemblyPath, ConnectionString);
            deployManager.UnregisterAsDNDBT(ConnectionString);

            SQLiteDatabaseInfo dbInfoFromDbAssembly = (SQLiteDatabaseInfo)DbDefinitionParser.CreateDatabaseInfo(s_sqliteSampleDbAssemblyPath);
            SQLiteDatabaseInfo dbInfoFromDBMSSysInfo = (SQLiteDatabaseInfo)interactor.GenerateDatabaseModelFromDBMSSysInfo();

            dbInfoFromDBMSSysInfo.Should().BeEquivalentTo(dbInfoFromDbAssembly, options => options
                .Excluding(dbInfo => dbInfo.Name)
                .Excluding(dbInfo => dbInfo.Views)
                .Excluding(dbInfo => dbInfo.Path.EndsWith(".ID", StringComparison.Ordinal)));
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
