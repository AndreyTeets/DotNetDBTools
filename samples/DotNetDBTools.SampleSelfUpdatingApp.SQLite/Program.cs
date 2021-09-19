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
        private const string RepoRoot = "../../../../..";

        private static readonly string s_agnosticDbProjectBinDir = $"{RepoRoot}/samples/DotNetDBTools.SampleDB.Agnostic/bin";
        private static readonly string s_agnosticDbAssemblyPath = $"{s_agnosticDbProjectBinDir}/DbAssembly/DotNetDBTools.SampleDB.Agnostic.dll";
        private static readonly string s_agnosticDbFilePath = $"./tmp/AgnosticSampleDB_SelfUpdatingApp.db";
        private static readonly string s_agnosticConnectionString = $"DataSource={s_agnosticDbFilePath};Mode=ReadWriteCreate;";

        public static void Main()
        {
            SqliteConnection dbConnection = new(s_agnosticConnectionString);
            SqliteCompiler compiler = new();

            DropDatabaseIfExists(s_agnosticDbFilePath);

            Console.WriteLine("Creating new AgnosticSampleDB...");
            DeployAgnosticSampleDB();

            SampleBusinessLogic.ReadWriteSomeData(dbConnection, compiler);
        }

        private static void DeployAgnosticSampleDB()
        {
            SQLiteDeployManager deployManager = new(true, false);
            deployManager.PublishDatabase(s_agnosticDbAssemblyPath, s_agnosticConnectionString);
        }

        private static void DropDatabaseIfExists(string dbFilePath)
        {
            if (File.Exists(dbFilePath))
                File.Delete(dbFilePath);
            Directory.CreateDirectory(Path.GetDirectoryName(dbFilePath));
        }
    }
}
