using System;
using System.Data.Common;
using System.IO;
using DotNetDBTools.Deploy;
using DotNetDBTools.EventsLogger;
using DotNetDBTools.SampleBusinessLogicLib.Agnostic;
using Microsoft.Data.Sqlite;
using SqlKata.Compilers;

namespace DotNetDBTools.SampleSelfUpdatingApp.SQLite
{
    public static class Program
    {
        private const string RepoRoot = "../../../../../..";
        private static readonly string s_samplesOutputDir = $"{RepoRoot}/SamplesOutput";

        private static readonly string s_connectionString = $"DataSource={s_samplesOutputDir}/sqlite_databases/AgnosticSampleDB_SelfUpdatingApp.db;Mode=ReadWriteCreate;";

        public static void Main()
        {
            CreateAndRegisterDatabaseIfDoesntExist(s_connectionString);

            using SqliteConnection connection = new(s_connectionString);
            PublishAgnosticSampleDBv2(connection);

            SqliteCompiler compiler = new();
            SampleBusinessLogic.ReadWriteSomeData(connection, compiler);
        }

        private static void PublishAgnosticSampleDBv2(DbConnection connection)
        {
            Console.WriteLine("Publishing DotNetDBTools.SampleDBv2.Agnostic from referenced assembly");
            SQLiteDeployManager deployManager = new(new DeployOptions());
            deployManager.Events.EventFired += DeployManagerEventsLogger.LogEvent;
            deployManager.PublishDatabase(typeof(SampleDB.Agnostic.Tables.MyTable3).Assembly, connection);
        }

        private static void CreateAndRegisterDatabaseIfDoesntExist(string connectionString)
        {
            if (!DatabaseExists(connectionString))
            {
                Console.WriteLine("Database doesn't exist. Creating new empty database and registering it as DNDBT.");
                CreateDatabase(connectionString);
                SQLiteDeployManager deployManager = new(new DeployOptions());
                deployManager.Events.EventFired += DeployManagerEventsLogger.LogEvent;
                using SqliteConnection connection = new(connectionString);
                deployManager.RegisterAsDNDBT(connection);
            }
        }

        private static bool DatabaseExists(string connectionString)
        {
            SqliteConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string dbFilePath = connectionStringBuilder.DataSource;
            return File.Exists(dbFilePath);
        }

        private static void CreateDatabase(string connectionString)
        {
            SqliteConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string dbFilePath = connectionStringBuilder.DataSource;
            Directory.CreateDirectory(Path.GetDirectoryName(dbFilePath));
        }
    }
}
