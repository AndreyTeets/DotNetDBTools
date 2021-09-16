using System.IO;
using DotNetDBTools.Deploy.SQLite;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetDBTools.IntegrationTests.SQLite
{
    [TestClass]
    public class SQLiteDeployTests
    {
        private const string AgnosticSampleDbAssemblyPath = "../../../../../Samples/DotNetDBTools.SampleDB.Agnostic/bin/DbAssembly/DotNetDBTools.SampleDB.Agnostic.dll";
        private const string SQLiteSampleDbAssemblyPath = "../../../../../Samples/DotNetDBTools.SampleDB.SQLite/bin/DbAssembly/DotNetDBTools.SampleDB.SQLite.dll";
        private const string DbFilesFolder = @".\tmp";

        private string ConnectionString => $@"DataSource={DbFilesFolder}\{TestContext.TestName}.db;Mode=ReadWriteCreate;";
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Update_AgnosticSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            DropDatabaseIfExists(ConnectionString);
            SQLiteDeployManager deployManager = new(true, false);
            deployManager.UpdateDatabase(AgnosticSampleDbAssemblyPath, ConnectionString);
            deployManager.UpdateDatabase(AgnosticSampleDbAssemblyPath, ConnectionString);
        }

        [TestMethod]
        public void Update_SQLiteSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            DropDatabaseIfExists(ConnectionString);
            SQLiteDeployManager deployManager = new(true, false);
            deployManager.UpdateDatabase(SQLiteSampleDbAssemblyPath, ConnectionString);
            deployManager.UpdateDatabase(SQLiteSampleDbAssemblyPath, ConnectionString);
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
