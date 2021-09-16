using System.IO;
using DotNetDBTools.Deploy.SQLite;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotNetDBTools.TestsUtils.Constants;

namespace DotNetDBTools.IntegrationTests.SQLite
{
    [TestClass]
    public class SQLiteDeployTests
    {
        private const string DbFilesFolder = @".\tmp";

        private static readonly string s_agnosticSampleDbAssemblyPath = $"{RepoRoot}/samples/Databases/DotNetDBTools.SampleDB.Agnostic/bin/DbAssembly/DotNetDBTools.SampleDB.Agnostic.dll";
        private static readonly string s_sqliteSampleDbAssemblyPath = $"{RepoRoot}/samples/Databases/DotNetDBTools.SampleDB.SQLite/bin/DbAssembly/DotNetDBTools.SampleDB.SQLite.dll";

        private string ConnectionString => $@"DataSource={DbFilesFolder}\{TestContext.TestName}.db;Mode=ReadWriteCreate;";
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Update_AgnosticSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            DropDatabaseIfExists(ConnectionString);
            SQLiteDeployManager deployManager = new(true, false);
            deployManager.UpdateDatabase(s_agnosticSampleDbAssemblyPath, ConnectionString);
            deployManager.UpdateDatabase(s_agnosticSampleDbAssemblyPath, ConnectionString);
        }

        [TestMethod]
        public void Update_SQLiteSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            DropDatabaseIfExists(ConnectionString);
            SQLiteDeployManager deployManager = new(true, false);
            deployManager.UpdateDatabase(s_sqliteSampleDbAssemblyPath, ConnectionString);
            deployManager.UpdateDatabase(s_sqliteSampleDbAssemblyPath, ConnectionString);
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
