using System.IO;
using DotNetDBTools.Deploy;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.SQLite
{
    [TestClass]
    public class SQLiteDeployTests
    {
        private const string DbFilesFolder = @".\tmp";

        private static readonly string s_agnosticSampleDbAssemblyPath = $"{SamplesOutputDir}/DotNetDBTools.SampleDB.Agnostic.dll";
        private static readonly string s_sqliteSampleDbAssemblyPath = $"{SamplesOutputDir}/DotNetDBTools.SampleDB.SQLite.dll";

        private string ConnectionString => $@"DataSource={DbFilesFolder}\{TestContext.TestName}.db;Mode=ReadWriteCreate;";
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
        public void Publish_SQLiteSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            DropDatabaseIfExists(ConnectionString);
            SQLiteDeployManager deployManager = new(true, false);
            deployManager.PublishDatabase(s_sqliteSampleDbAssemblyPath, ConnectionString);
            deployManager.PublishDatabase(s_sqliteSampleDbAssemblyPath, ConnectionString);
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
