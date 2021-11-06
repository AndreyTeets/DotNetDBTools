using System;
using System.IO;
using DotNetDBTools.Deploy;
using DotNetDBTools.SampleBusinessLogicLib.Agnostic;
using Microsoft.Data.Sqlite;
using SqlKata.Compilers;

namespace DotNetDBTools.SampleSelfUpdatingApp.SQLite
{
    public static class Program
    {
        private const string RepoRoot = "../../../../../..";
        private static readonly string s_samplesOutputDir = $"{RepoRoot}/SamplesOutput";

        private static readonly string s_agnosticConnectionString = $"DataSource={s_samplesOutputDir}/sqlite_databases/AgnosticSampleDB_SelfUpdatingApp.db;Mode=ReadWriteCreate;";

        public static void Main()
        {
            DropDatabaseIfExists(s_agnosticConnectionString);

            Console.WriteLine("Creating new AgnosticSampleDB_SelfUpdatingApp v2 from dbAssembly file...");
            DeployAgnosticSampleDB();

            SqliteConnection dbConnection = new(s_agnosticConnectionString);
            SqliteCompiler compiler = new();
            SampleBusinessLogic.ReadWriteSomeData(dbConnection, compiler);
        }

        private static void DeployAgnosticSampleDB()
        {
            SQLiteDeployManager deployManager = new(new DeployOptions { AllowDbCreation = true });
            deployManager.PublishDatabase(typeof(SampleDB.Agnostic.Tables.MyTable3).Assembly, s_agnosticConnectionString);
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
