using System;
using System.IO;
using System.Reflection;
using Dapper;
using DotNetDBTools.Deploy;
using DotNetDBTools.EventsLogger;
using DotNetDBTools.Generation;
using Microsoft.Data.Sqlite;

namespace DotNetDBTools.SampleDeployManagerUsage.SQLite
{
    public static class Program
    {
        private const string RepoRoot = "../../../../../..";
        private static readonly string s_samplesOutputDir = $"{RepoRoot}/SamplesOutput";
        private static readonly string s_generatedOutputDir = $"{s_samplesOutputDir}/sqlite_generated";

        private static readonly string s_sqliteDbAssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDB.SQLite.dll";
        private static readonly string s_sqliteDbV2AssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDBv2.SQLite.dll";
        private static readonly string s_sqliteDbV2SqlDefAssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDBv2SqlDef.SQLite.dll";
        private static readonly string s_sqliteConnectionString = $"DataSource={s_samplesOutputDir}/sqlite_databases/SQLiteSampleDB_BusinessLogicOnlyApp.db;Mode=ReadWriteCreate;";

        private static readonly string s_publishToEmptyScriptPath = $"{s_generatedOutputDir}/PublishToEmptyScript.sql";
        private static readonly string s_publishToExistingScriptPath = $"{s_generatedOutputDir}/PublishToExistingScript.sql";
        private static readonly string s_publishFromV1ToV2ScriptPath = $"{s_generatedOutputDir}/PublishFromV1ToV2Script.sql";
        private static readonly string s_noDNDBTInfoPublishFromV1ToV2ScriptPath = $"{s_generatedOutputDir}/NoDNDBTInfoPublishFromV1ToV2Script.sql";
        private static readonly string s_publishFromV1ToV2SqlDefScriptPath = $"{s_generatedOutputDir}/PublishFromV1ToV2SqlDefScript.sql";
        private static readonly string s_publishFromV2ToV1ScriptPath = $"{s_generatedOutputDir}/PublishFromV2ToV1Script.sql";
        private static readonly string s_noDNDBTInfoPublishFromV2ToV1ScriptPath = $"{s_generatedOutputDir}/NoDNDBTInfoPublishFromV2ToV1Script.sql";
        private static readonly string s_definitionFromUnregisteredDir = $"{s_generatedOutputDir}/DefinitionFromUnregistered";
        private static readonly string s_definitionFromRegisteredDir = $"{s_generatedOutputDir}/DefinitionFromRegistered";
        private static readonly string s_publishRecreateScriptPath = $"{s_generatedOutputDir}/PublishRecreateScript.sql";

        public static void Main()
        {
            RunSQLiteSampleDBDeployExamples();
        }

        private static void RunSQLiteSampleDBDeployExamples()
        {
            DropDatabaseIfExists(s_sqliteConnectionString);
            Directory.CreateDirectory(s_generatedOutputDir);

            Assembly dbAssembly = Assembly.Load(File.ReadAllBytes(s_sqliteDbAssemblyPath));
            Assembly dbAssemblyV2 = Assembly.Load(File.ReadAllBytes(s_sqliteDbV2AssemblyPath));

            IDeployManager deployManager = new SQLiteDeployManager(new DeployOptions());
            deployManager.Events.EventFired += DeployManagerEventsLogger.LogEvent;
            IDeployManager dmDataLoss = new SQLiteDeployManager(new DeployOptions { AllowDataLoss = true });
            dmDataLoss.Events.EventFired += DeployManagerEventsLogger.LogEvent;
            using SqliteConnection connection = new(s_sqliteConnectionString);

            GenerationOptions generationOptions = new()
            {
                DatabaseName = "CustomDbName",
                OutputDefinitionKind = OutputDefinitionKind.Sql
            };

            Console.WriteLine("Registering empty SQLiteSampleDB as DNDBT to make other actions with it possible...");
            deployManager.RegisterAsDNDBT(connection);

            Console.WriteLine("Generating script to create new SQLiteSampleDB from dbAssembly file...");
            File.WriteAllText(s_publishToEmptyScriptPath, deployManager.GeneratePublishScript(s_sqliteDbAssemblyPath, connection));
            Console.WriteLine("Creating new SQLiteSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_sqliteDbAssemblyPath, connection);

            Console.WriteLine("Generating script to update(no changes) existing SQLiteSampleDB from dbAssembly file...");
            File.WriteAllText(s_publishToExistingScriptPath, deployManager.GeneratePublishScript(s_sqliteDbAssemblyPath, connection));
            Console.WriteLine("Updating(no changes) existing SQLiteSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_sqliteDbAssemblyPath, connection);

            Console.WriteLine("Generating script to update(from v1 to v2) SQLiteSampleDB from the corresponding assembly files...");
            File.WriteAllText(s_publishFromV1ToV2ScriptPath, dmDataLoss.GeneratePublishScript(dbAssemblyV2, dbAssembly));
            File.WriteAllText(s_publishFromV1ToV2SqlDefScriptPath, dmDataLoss.GeneratePublishScript(s_sqliteDbV2SqlDefAssemblyPath, s_sqliteDbAssemblyPath));
            Console.WriteLine("Updating(from v1 to v2) existing SQLiteSampleDB from dbAssembly v2 file...");
            dmDataLoss.PublishDatabase(s_sqliteDbV2AssemblyPath, connection);

            Console.WriteLine("Generating script to update(rollback from v2 to v1) existing SQLiteSampleDB from dbAssembly file...");
            File.WriteAllText(s_publishFromV2ToV1ScriptPath, dmDataLoss.GeneratePublishScript(s_sqliteDbAssemblyPath, connection));
            Console.WriteLine("Updating(rollback from v2 to v1) SQLiteSampleDB using previously generated script...");
            connection.Execute(File.ReadAllText(s_publishFromV2ToV1ScriptPath));

            Console.WriteLine("Unregistering(=deleting DNDBT system information from DB) SQLiteSampleDB...");
            deployManager.UnregisterAsDNDBT(connection);
            Console.WriteLine("Generating definition from existing unregistered SQLiteSampleDB...");
            deployManager.GenerateDefinition(connection, s_definitionFromUnregisteredDir);
            deployManager.GenerateDefinition(connection, generationOptions, s_definitionFromUnregisteredDir + "SqlDef");

            Console.WriteLine("Generating NoDNDBTInfo script to update(from v1 to v2) SQLiteSampleDB from the corresponding assembly files...");
            File.WriteAllText(s_noDNDBTInfoPublishFromV1ToV2ScriptPath, dmDataLoss.GenerateNoDNDBTInfoPublishScript(dbAssemblyV2, dbAssembly));
            Console.WriteLine("Updating(from v1 to v2) SQLiteSampleDB using previously generated NoDNDBTInfo script...");
            connection.Execute(File.ReadAllText(s_noDNDBTInfoPublishFromV1ToV2ScriptPath));

            Console.WriteLine("Generating NoDNDBTInfo script to update(rollback from v2 to v1) SQLiteSampleDB from the corresponding assembly files...");
            File.WriteAllText(s_noDNDBTInfoPublishFromV2ToV1ScriptPath, dmDataLoss.GenerateNoDNDBTInfoPublishScript(dbAssembly, dbAssemblyV2));
            Console.WriteLine("Updating(rollback from v2 to v1) SQLiteSampleDB using previously generated NoDNDBTInfo script...");
            connection.Execute(File.ReadAllText(s_noDNDBTInfoPublishFromV2ToV1ScriptPath));

            Console.WriteLine("Registering(=generating and adding new DNDBT system information to DB) SQLiteSampleDB...");
            deployManager.RegisterAsDNDBT(connection);
            Console.WriteLine("Generating definition from existing registered SQLiteSampleDB...");
            deployManager.GenerateDefinition(connection, s_definitionFromRegisteredDir);
            deployManager.GenerateDefinition(connection, generationOptions, s_definitionFromRegisteredDir + "SqlDef");

            Console.WriteLine("Newly created DNDBT system information(by registration) has different IDs for all objects");
            Console.WriteLine("Generating script to update(recreate all objects) existing SQLiteSampleDB from dbAssembly file...");
            File.WriteAllText(s_publishRecreateScriptPath, dmDataLoss.GeneratePublishScript(s_sqliteDbAssemblyPath, connection));
            Console.WriteLine("Updating(recreate all objects) existing SQLiteSampleDB from dbAssembly file...");
            dmDataLoss.PublishDatabase(s_sqliteDbAssemblyPath, connection);
        }

        private static void DropDatabaseIfExists(string connectionString)
        {
            SqliteConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string dbFilePath = connectionStringBuilder.DataSource;
            if (File.Exists(dbFilePath))
                File.Delete(dbFilePath);
            Directory.CreateDirectory(Path.GetDirectoryName(dbFilePath));
        }
    }
}
