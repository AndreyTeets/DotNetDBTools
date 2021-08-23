using System;
using System.IO;
using System.Reflection;
using DotNetDBTools.Deploy.SQLite;
using Xunit;

namespace DotNetDBTools.IntegrationTests.SQLite
{
    public class SQLiteDeployTests : IDisposable
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
        private const string DbFilePath = @".\tmp\SampleDB.db";
        private static readonly string s_connectionString = $"DataSource={DbFilePath};Mode=ReadWriteCreate;";

        public SQLiteDeployTests()
        {
            DropDatabaseIfExists(DbFilePath);
            Directory.CreateDirectory(Path.GetDirectoryName(DbFilePath));
        }

        public void Dispose()
        {
            DropDatabaseIfExists(DbFilePath);
        }

        [Fact]
        public void Sample_Projects_Run_OnNew_And_OnExisting_Databases_WithoutErrors()
        {
            ProcessRunner processRunner = new();

            (int exitCodeDeploy, string outputDeploy) = processRunner.RunProcess(s_deployAssemblyPath);
            Assert.True(exitCodeDeploy == 0, outputDeploy);

            (int exitCodeAgnosticApplication, string outputAgnosticApplication) = processRunner.RunProcess(s_applicationAssemblyPath);
            Assert.True(exitCodeAgnosticApplication == 0, outputAgnosticApplication);
        }

        [Fact]
        public void Update_AgnosticSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            Assembly dbAssembly = Assembly.GetAssembly(typeof(SampleDB.Agnostic.Tables.MyTable1));
            SQLiteDeployManager deployManager = new(true);
            deployManager.UpdateDatabase(dbAssembly, s_connectionString);
            deployManager.UpdateDatabase(dbAssembly, s_connectionString);
        }

        [Fact]
        public void Update_SQLiteSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            Assembly dbAssembly = Assembly.GetAssembly(typeof(SampleDB.SQLite.Tables.MyTable1));
            SQLiteDeployManager deployManager = new(true);
            deployManager.UpdateDatabase(dbAssembly, s_connectionString);
            deployManager.UpdateDatabase(dbAssembly, s_connectionString);
        }

        private static void DropDatabaseIfExists(string dbFilePath)
        {
            if (File.Exists(dbFilePath))
                File.Delete(dbFilePath);
        }
    }
}
