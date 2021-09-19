using System;
using System.IO;
using DotNetDBTools.Deploy;
using Microsoft.Data.Sqlite;

namespace DotNetDBTools.SampleDeployTool.SQLite
{
    public class Program
    {
        private const string AgnosticGeneratedPublishToEmptyScriptPath = "./generated/AgnosticGeneratedPublishToEmptyScript.sql";
        private const string AgnosticGeneratedPublishToExistingScriptPath = "./generated/AgnosticGeneratedPublishToExistingScript.sql";
        private const string AgnosticGeneratedDefinitionFromUnregisteredDirectory = "./generated/AgnosticGeneratedDefinitionFromUnregisteredDirectory";
        private const string AgnosticGeneratedDefinitionFromRegisteredDirectory = "./generated/AgnosticGeneratedDefinitionFromRegisteredDirectory";
        private const string SQLiteGeneratedPublishToEmptyScriptPath = "./generated/SQLiteGeneratedPublishToEmptyScript.sql";
        private const string SQLiteGeneratedPublishToExistingScriptPath = "./generated/SQLiteGeneratedPublishToExistingScript.sql";
        private const string SQLiteGeneratedDefinitionFromUnregisteredDirectory = "./generated/SQLiteGeneratedDefinitionFromUnregisteredDirectory";
        private const string SQLiteGeneratedDefinitionFromRegisteredDirectory = "./generated/SQLiteGeneratedDefinitionFromRegisteredDirectory";
        private const string RepoRoot = "../../../../..";

        private static readonly string s_agnosticDbProjectBinDir = $"{RepoRoot}/samples/DotNetDBTools.SampleDB.Agnostic/bin";
        private static readonly string s_agnosticDbAssemblyPath = $"{s_agnosticDbProjectBinDir}/DbAssembly/DotNetDBTools.SampleDB.Agnostic.dll";
        private static readonly string s_agnosticDbFilePath = $"{s_agnosticDbProjectBinDir}/DbFile/AgnosticSampleDB.db";
        private static readonly string s_agnosticConnectionString = $"DataSource={s_agnosticDbFilePath};Mode=ReadWriteCreate;";

        private static readonly string s_sqliteDbProjectBinDir = $"{RepoRoot}/samples/DotNetDBTools.SampleDB.SQLite/bin";
        private static readonly string s_sqliteDbAssemblyPath = $"{s_sqliteDbProjectBinDir}/DbAssembly/DotNetDBTools.SampleDB.SQLite.dll";
        private static readonly string s_sqliteDbFilePath = $"{s_sqliteDbProjectBinDir}/DbFile/SQLiteSampleDB.db";
        private static readonly string s_sqliteConnectionString = $"DataSource={s_sqliteDbFilePath};Mode=ReadWriteCreate;";

        public static void Main()
        {
            RunAgnosticSampleDBDeployExamples();
            RunSQLiteSampleDBDeployExamples();
        }

        private static void RunAgnosticSampleDBDeployExamples()
        {
            DropDatabaseIfExists(s_agnosticConnectionString);
            IDeployManager deployManager = new SQLiteDeployManager(true, false);

            Console.WriteLine("Generating create new AgnosticSampleDB from dbAssembly file...");
            deployManager.GeneratePublishScript(s_agnosticDbAssemblyPath, s_agnosticConnectionString, AgnosticGeneratedPublishToEmptyScriptPath);
            Console.WriteLine("Creating new AgnosticSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_agnosticDbAssemblyPath, s_agnosticConnectionString);

            Console.WriteLine("Generating update(no changes) existing AgnosticSampleDB from dbAssembly file...");
            deployManager.GeneratePublishScript(s_agnosticDbAssemblyPath, s_agnosticConnectionString, AgnosticGeneratedPublishToExistingScriptPath);
            Console.WriteLine("Updating(no changes) existing AgnosticSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_agnosticDbAssemblyPath, s_agnosticConnectionString);

            Console.WriteLine("Unregistering(=deleting DNDBT system information from DB) AgnosticSampleDB...");
            deployManager.UnregisterAsDNDBT(s_agnosticConnectionString);
            Console.WriteLine("Generating definition from existing unregistered AgnosticSampleDB...");
            deployManager.GenerateDefinition(s_agnosticConnectionString, AgnosticGeneratedDefinitionFromUnregisteredDirectory);

            Console.WriteLine("Registiring(=generating and adding new DNDBT system information to DB) AgnosticSampleDB...");
            deployManager.RegisterAsDNDBT(s_agnosticConnectionString);
            Console.WriteLine("Generating definition from existing registered AgnosticSampleDB...");
            deployManager.GenerateDefinition(s_agnosticConnectionString, AgnosticGeneratedDefinitionFromRegisteredDirectory);
        }

        private static void RunSQLiteSampleDBDeployExamples()
        {
            DropDatabaseIfExists(s_sqliteConnectionString);
            IDeployManager deployManager = new SQLiteDeployManager(true, false);

            Console.WriteLine("Generating create new SQLiteSampleDB from dbAssembly file...");
            deployManager.GeneratePublishScript(s_sqliteDbAssemblyPath, s_sqliteConnectionString, SQLiteGeneratedPublishToEmptyScriptPath);
            Console.WriteLine("Creating new SQLiteSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_sqliteDbAssemblyPath, s_sqliteConnectionString);

            Console.WriteLine("Generating update(no changes) existing SQLiteSampleDB from dbAssembly file...");
            deployManager.GeneratePublishScript(s_sqliteDbAssemblyPath, s_sqliteConnectionString, SQLiteGeneratedPublishToExistingScriptPath);
            Console.WriteLine("Updating(no changes) existing SQLiteSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_sqliteDbAssemblyPath, s_sqliteConnectionString);

            Console.WriteLine("Unregistering(=deleting DNDBT system information from DB) SQLiteSampleDB...");
            deployManager.UnregisterAsDNDBT(s_sqliteConnectionString);
            Console.WriteLine("Generating definition from existing unregistered SQLiteSampleDB...");
            deployManager.GenerateDefinition(s_sqliteConnectionString, SQLiteGeneratedDefinitionFromUnregisteredDirectory);

            Console.WriteLine("Registiring(=generating and adding new DNDBT system information to DB) SQLiteSampleDB...");
            deployManager.RegisterAsDNDBT(s_sqliteConnectionString);
            Console.WriteLine("Generating definition from existing registered SQLiteSampleDB...");
            deployManager.GenerateDefinition(s_sqliteConnectionString, SQLiteGeneratedDefinitionFromRegisteredDirectory);
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
