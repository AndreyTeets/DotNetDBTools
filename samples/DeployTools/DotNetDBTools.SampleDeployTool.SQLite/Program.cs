using System;
using System.IO;
using DotNetDBTools.Deploy;
using Microsoft.Data.Sqlite;

namespace DotNetDBTools.SampleDeployTool.SQLite
{
    public class Program
    {
        private const string RepoRoot = "../../../../../..";
        private static readonly string s_samplesOutputDir = $"{RepoRoot}/SamplesOutput";

        private static readonly string s_agnosticDbAssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDB.Agnostic.dll";
        private static readonly string s_agnosticDbV2AssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDBv2.Agnostic.dll";
        private static readonly string s_agnosticConnectionString = $"DataSource={s_samplesOutputDir}/sqlite_databases/AgnosticSampleDB.db;Mode=ReadWriteCreate;";

        private static readonly string s_sqliteDbAssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDB.SQLite.dll";
        private static readonly string s_sqliteDbV2AssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDBv2.SQLite.dll";
        private static readonly string s_sqliteConnectionString = $"DataSource={s_samplesOutputDir}/sqlite_databases/SQLiteSampleDB.db;Mode=ReadWriteCreate;";

        private static readonly string s_agnosticGeneratedPublishToEmptyScriptPath = $"{s_samplesOutputDir}/sqlite_generated/AgnosticGeneratedPublishToEmptyScript.sql";
        private static readonly string s_agnosticGeneratedPublishToExistingScriptPath = $"{s_samplesOutputDir}/sqlite_generated/AgnosticGeneratedPublishToExistingScript.sql";
        private static readonly string s_agnosticGeneratedPublishFromV1ToV2ScriptPath = $"{s_samplesOutputDir}/sqlite_generated/AgnosticGeneratedPublishFromV1ToV2Script.sql";
        private static readonly string s_agnosticGeneratedPublishFromV2ToV1ScriptPath = $"{s_samplesOutputDir}/sqlite_generated/AgnosticGeneratedPublishFromV2ToV1Script.sql";
        private static readonly string s_agnosticGeneratedDefinitionFromUnregisteredDir = $"{s_samplesOutputDir}/sqlite_generated/AgnosticGeneratedDefinitionFromUnregistered";
        private static readonly string s_agnosticGeneratedDefinitionFromRegisteredDir = $"{s_samplesOutputDir}/sqlite_generated/AgnosticGeneratedDefinitionFromRegistered";
        private static readonly string s_agnosticGeneratedPublishRecreateScriptPath = $"{s_samplesOutputDir}/sqlite_generated/AgnosticGeneratedPublishRecreateScript.sql";

        private static readonly string s_sqliteGeneratedPublishToEmptyScriptPath = $"{s_samplesOutputDir}/sqlite_generated/SQLiteGeneratedPublishToEmptyScript.sql";
        private static readonly string s_sqliteGeneratedPublishToExistingScriptPath = $"{s_samplesOutputDir}/sqlite_generated/SQLiteGeneratedPublishToExistingScript.sql";
        private static readonly string s_sqliteGeneratedPublishFromV1ToV2ScriptPath = $"{s_samplesOutputDir}/sqlite_generated/SQLiteGeneratedPublishFromV1ToV2Script.sql";
        private static readonly string s_sqliteGeneratedPublishFromV2ToV1ScriptPath = $"{s_samplesOutputDir}/sqlite_generated/SQLiteGeneratedPublishFromV2ToV1Script.sql";
        private static readonly string s_sqliteGeneratedDefinitionFromUnregisteredDir = $"{s_samplesOutputDir}/sqlite_generated/SQLiteGeneratedDefinitionFromUnregistered";
        private static readonly string s_sqliteGeneratedDefinitionFromRegisteredDir = $"{s_samplesOutputDir}/sqlite_generated/SQLiteGeneratedDefinitionFromRegistered";
        private static readonly string s_sqliteGeneratedPublishRecreateScriptPath = $"{s_samplesOutputDir}/sqlite_generated/SQLiteGeneratedPublishRecreateScript.sql";

        public static void Main()
        {
            RunAgnosticSampleDBDeployExamples();
            RunSQLiteSampleDBDeployExamples();
        }

        private static void RunAgnosticSampleDBDeployExamples()
        {
            DropDatabaseIfExists(s_agnosticConnectionString);
            IDeployManager deployManager = new SQLiteDeployManager(true, false);
            IDeployManager dmDataLoss = new SQLiteDeployManager(false, true);

            Console.WriteLine("Generating script to create new AgnosticSampleDB from dbAssembly file...");
            deployManager.GeneratePublishScript(s_agnosticDbAssemblyPath, s_agnosticConnectionString, s_agnosticGeneratedPublishToEmptyScriptPath);
            Console.WriteLine("Creating new AgnosticSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_agnosticDbAssemblyPath, s_agnosticConnectionString);

            Console.WriteLine("Generating script to update(no changes) existing AgnosticSampleDB from dbAssembly file...");
            deployManager.GeneratePublishScript(s_agnosticDbAssemblyPath, s_agnosticConnectionString, s_agnosticGeneratedPublishToExistingScriptPath);
            Console.WriteLine("Updating(no changes) existing AgnosticSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_agnosticDbAssemblyPath, s_agnosticConnectionString);

            Console.WriteLine("Generating script to update(from v1 to v2) existing AgnosticSampleDB from dbAssembly v2 file...");
            deployManager.GeneratePublishScript(s_agnosticDbV2AssemblyPath, s_agnosticConnectionString, s_agnosticGeneratedPublishFromV1ToV2ScriptPath);
            Console.WriteLine("Updating(from v1 to v2) existing AgnosticSampleDB from dbAssembly v2 file...");
            deployManager.PublishDatabase(s_agnosticDbV2AssemblyPath, s_agnosticConnectionString);

            Console.WriteLine("Generating script to update(rollback from v2 to v1) existing AgnosticSampleDB from dbAssembly file...");
            dmDataLoss.GeneratePublishScript(s_agnosticDbAssemblyPath, s_agnosticConnectionString, s_agnosticGeneratedPublishFromV2ToV1ScriptPath);
            Console.WriteLine("Updating(rollback from v2 to v1) existing AgnosticSampleDB from dbAssembly file...");
            dmDataLoss.PublishDatabase(s_agnosticDbAssemblyPath, s_agnosticConnectionString);

            Console.WriteLine("Unregistering(=deleting DNDBT system information from DB) AgnosticSampleDB...");
            deployManager.UnregisterAsDNDBT(s_agnosticConnectionString);
            Console.WriteLine("Generating definition from existing unregistered AgnosticSampleDB...");
            deployManager.GenerateDefinition(s_agnosticConnectionString, s_agnosticGeneratedDefinitionFromUnregisteredDir);

            Console.WriteLine("Registiring(=generating and adding new DNDBT system information to DB) AgnosticSampleDB...");
            deployManager.RegisterAsDNDBT(s_agnosticConnectionString);
            Console.WriteLine("Generating definition from existing registered AgnosticSampleDB...");
            deployManager.GenerateDefinition(s_agnosticConnectionString, s_agnosticGeneratedDefinitionFromRegisteredDir);

            Console.WriteLine("Newly created DNDBT system information(by registration) has different IDs for all objects");
            Console.WriteLine("Generating script to update(recreate all objects) existing AgnosticSampleDB from dbAssembly file...");
            dmDataLoss.GeneratePublishScript(s_agnosticDbAssemblyPath, s_agnosticConnectionString, s_agnosticGeneratedPublishRecreateScriptPath);
            Console.WriteLine("Updating(recreate all objects) existing AgnosticSampleDB from dbAssembly file...");
            dmDataLoss.PublishDatabase(s_agnosticDbAssemblyPath, s_agnosticConnectionString);
        }

        private static void RunSQLiteSampleDBDeployExamples()
        {
            DropDatabaseIfExists(s_sqliteConnectionString);
            IDeployManager deployManager = new SQLiteDeployManager(true, false);
            IDeployManager dmDataLoss = new SQLiteDeployManager(false, true);

            Console.WriteLine("Generating script to create new SQLiteSampleDB from dbAssembly file...");
            deployManager.GeneratePublishScript(s_sqliteDbAssemblyPath, s_sqliteConnectionString, s_sqliteGeneratedPublishToEmptyScriptPath);
            Console.WriteLine("Creating new SQLiteSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_sqliteDbAssemblyPath, s_sqliteConnectionString);

            Console.WriteLine("Generating script to update(no changes) existing SQLiteSampleDB from dbAssembly file...");
            deployManager.GeneratePublishScript(s_sqliteDbAssemblyPath, s_sqliteConnectionString, s_sqliteGeneratedPublishToExistingScriptPath);
            Console.WriteLine("Updating(no changes) existing SQLiteSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_sqliteDbAssemblyPath, s_sqliteConnectionString);

            Console.WriteLine("Generating script to update(from v1 to v2) existing SQLiteSampleDB from dbAssembly v2 file...");
            deployManager.GeneratePublishScript(s_sqliteDbV2AssemblyPath, s_sqliteConnectionString, s_sqliteGeneratedPublishFromV1ToV2ScriptPath);
            Console.WriteLine("Updating(from v1 to v2) existing SQLiteSampleDB from dbAssembly v2 file...");
            deployManager.PublishDatabase(s_sqliteDbV2AssemblyPath, s_sqliteConnectionString);

            Console.WriteLine("Generating script to update(rollback from v2 to v1) existing SQLiteSampleDB from dbAssembly file...");
            dmDataLoss.GeneratePublishScript(s_sqliteDbAssemblyPath, s_sqliteConnectionString, s_sqliteGeneratedPublishFromV2ToV1ScriptPath);
            Console.WriteLine("Updating(rollback from v2 to v1) existing SQLiteSampleDB from dbAssembly file...");
            dmDataLoss.PublishDatabase(s_sqliteDbAssemblyPath, s_sqliteConnectionString);

            Console.WriteLine("Unregistering(=deleting DNDBT system information from DB) SQLiteSampleDB...");
            deployManager.UnregisterAsDNDBT(s_sqliteConnectionString);
            Console.WriteLine("Generating definition from existing unregistered SQLiteSampleDB...");
            deployManager.GenerateDefinition(s_sqliteConnectionString, s_sqliteGeneratedDefinitionFromUnregisteredDir);

            Console.WriteLine("Registiring(=generating and adding new DNDBT system information to DB) SQLiteSampleDB...");
            deployManager.RegisterAsDNDBT(s_sqliteConnectionString);
            Console.WriteLine("Generating definition from existing registered SQLiteSampleDB...");
            deployManager.GenerateDefinition(s_sqliteConnectionString, s_sqliteGeneratedDefinitionFromRegisteredDir);

            Console.WriteLine("Newly created DNDBT system information(by registration) has different IDs for all objects");
            Console.WriteLine("Generating script to update(recreate all objects) existing AgnosticSampleDB from dbAssembly file...");
            dmDataLoss.GeneratePublishScript(s_sqliteDbAssemblyPath, s_sqliteConnectionString, s_sqliteGeneratedPublishRecreateScriptPath);
            Console.WriteLine("Updating(recreate all objects) existing AgnosticSampleDB from dbAssembly file...");
            dmDataLoss.PublishDatabase(s_sqliteDbAssemblyPath, s_sqliteConnectionString);
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
