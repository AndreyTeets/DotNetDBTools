using System.IO;
using System.Reflection;
using DotNetDBTools.Deploy.SQLite;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetDBTools.IntegrationTests.SQLite
{
    [TestClass]
    public class SQLiteDeployTests
    {
#if DEBUG
        private const string Configuration = "Debug";
#else
        private const string Configuration = "Release";
#endif
        private const string DeployAssemblyBinDir = "../../../../../Samples/DotNetDBTools.SampleDeployUtil.SQLite/bin";
        private const string AgnosticApplicationAssemblBinDir = "../../../../../Samples/DotNetDBTools.SampleApplication.Agnostic/bin";
        private static readonly string s_deployAssemblyPath = $"{DeployAssemblyBinDir}/{Configuration}/netcoreapp3.1/DotNetDBTools.SampleDeployUtil.SQLite.exe";
        private static readonly string s_applicationAssemblyPath = $"{AgnosticApplicationAssemblBinDir}/{Configuration}/netcoreapp3.1/DotNetDBTools.SampleApplication.Agnostic.exe";
        private const string DbFilesFolder = @".\tmp";

        private string ConnectionString => $@"DataSource={DbFilesFolder}\{TestContext.TestName}.db;Mode=ReadWriteCreate;";
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Sample_Projects_Run_OnNew_And_OnExisting_Databases_WithoutErrors()
        {
            ProcessRunner processRunner = new();

            (int exitCodeDeploy, string outputDeploy) = processRunner.RunProcess(s_deployAssemblyPath);
            exitCodeDeploy.Should().Be(0, $"process output: '{outputDeploy}'");

            (int exitCodeAgnosticApplication, string outputAgnosticApplication) = processRunner.RunProcess(s_applicationAssemblyPath);
            exitCodeAgnosticApplication.Should().Be(0, $"process output: '{outputAgnosticApplication}'");
        }

        [TestMethod]
        public void Update_AgnosticSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            DropDatabaseIfExists(ConnectionString);
            Assembly dbAssembly = Assembly.GetAssembly(typeof(SampleDB.Agnostic.Tables.MyTable1));
            SQLiteDeployManager deployManager = new(true, false);
            deployManager.UpdateDatabase(dbAssembly, ConnectionString);
            deployManager.UpdateDatabase(dbAssembly, ConnectionString);
        }

        [TestMethod]
        public void Update_SQLiteSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            DropDatabaseIfExists(ConnectionString);
            Assembly dbAssembly = Assembly.GetAssembly(typeof(SampleDB.SQLite.Tables.MyTable1));
            SQLiteDeployManager deployManager = new(true, false);
            deployManager.UpdateDatabase(dbAssembly, ConnectionString);
            deployManager.UpdateDatabase(dbAssembly, ConnectionString);
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
