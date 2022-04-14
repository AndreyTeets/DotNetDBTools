﻿using System;
using System.IO;
using System.Reflection;
using Dapper;
using DotNetDBTools.Deploy;
using DotNetDBTools.EventsLogger;
using Microsoft.Data.Sqlite;

namespace DotNetDBTools.SampleDeployManagerUsage.SQLite
{
    public static class Program
    {
        private const string RepoRoot = "../../../../../..";
        private static readonly string s_samplesOutputDir = $"{RepoRoot}/SamplesOutput";

        private static readonly string s_sqliteDbAssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDB.SQLite.dll";
        private static readonly string s_sqliteDbV2AssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDBv2.SQLite.dll";
        private static readonly string s_sqliteDbV2SqlDefAssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDBv2SqlDef.SQLite.dll";
        private static readonly string s_sqliteConnectionString = $"DataSource={s_samplesOutputDir}/sqlite_databases/SQLiteSampleDB_BusinessLogicOnlyApp.db;Mode=ReadWriteCreate;";

        private static readonly string s_sqliteGeneratedPublishToEmptyScriptPath = $"{s_samplesOutputDir}/sqlite_generated/SQLiteGeneratedPublishToEmptyScript.sql";
        private static readonly string s_sqliteGeneratedPublishToExistingScriptPath = $"{s_samplesOutputDir}/sqlite_generated/SQLiteGeneratedPublishToExistingScript.sql";
        private static readonly string s_sqliteGeneratedPublishFromV1ToV2ScriptPath = $"{s_samplesOutputDir}/sqlite_generated/SQLiteGeneratedPublishFromV1ToV2Script.sql";
        private static readonly string s_sqliteGeneratedNoDNDBTInfoPublishFromV1ToV2ScriptPath = $"{s_samplesOutputDir}/sqlite_generated/SQLiteGeneratedNoDNDBTInfoPublishFromV1ToV2Script.sql";
        private static readonly string s_sqliteGeneratedPublishFromV1ToV2SqlDefScriptPath = $"{s_samplesOutputDir}/sqlite_generated/SQLiteGeneratedPublishFromV1ToV2SqlDefScript.sql";
        private static readonly string s_sqliteGeneratedPublishFromV2ToV1ScriptPath = $"{s_samplesOutputDir}/sqlite_generated/SQLiteGeneratedPublishFromV2ToV1Script.sql";
        private static readonly string s_sqliteGeneratedNoDNDBTInfoPublishFromV2ToV1ScriptPath = $"{s_samplesOutputDir}/sqlite_generated/SQLiteGeneratedNoDNDBTInfoPublishFromV2ToV1Script.sql";
        private static readonly string s_sqliteGeneratedDefinitionFromUnregisteredDir = $"{s_samplesOutputDir}/sqlite_generated/SQLiteGeneratedDefinitionFromUnregistered";
        private static readonly string s_sqliteGeneratedDefinitionFromRegisteredDir = $"{s_samplesOutputDir}/sqlite_generated/SQLiteGeneratedDefinitionFromRegistered";
        private static readonly string s_sqliteGeneratedPublishRecreateScriptPath = $"{s_samplesOutputDir}/sqlite_generated/SQLiteGeneratedPublishRecreateScript.sql";

        public static void Main()
        {
            RunSQLiteSampleDBDeployExamples();
        }

        private static void RunSQLiteSampleDBDeployExamples()
        {
            DropDatabaseIfExists(s_sqliteConnectionString);

            Assembly dbAssembly = Assembly.Load(File.ReadAllBytes(s_sqliteDbAssemblyPath));
            Assembly dbAssemblyV2 = Assembly.Load(File.ReadAllBytes(s_sqliteDbV2AssemblyPath));

            IDeployManager deployManager = new SQLiteDeployManager(new DeployOptions());
            deployManager.Events.EventFired += DeployManagerEventsLogger.LogEvent;
            IDeployManager dmDataLoss = new SQLiteDeployManager(new DeployOptions { AllowDataLoss = true });
            dmDataLoss.Events.EventFired += DeployManagerEventsLogger.LogEvent;
            using SqliteConnection connection = new(s_sqliteConnectionString);

            Console.WriteLine("Registering empty SQLiteSampleDB as DNDBT to make other actions with it possible...");
            deployManager.RegisterAsDNDBT(connection);

            Console.WriteLine("Generating script to create new SQLiteSampleDB from dbAssembly file...");
            deployManager.GeneratePublishScript(s_sqliteDbAssemblyPath, connection, s_sqliteGeneratedPublishToEmptyScriptPath);
            Console.WriteLine("Creating new SQLiteSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_sqliteDbAssemblyPath, connection);

            Console.WriteLine("Generating script to update(no changes) existing SQLiteSampleDB from dbAssembly file...");
            deployManager.GeneratePublishScript(s_sqliteDbAssemblyPath, connection, s_sqliteGeneratedPublishToExistingScriptPath);
            Console.WriteLine("Updating(no changes) existing SQLiteSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_sqliteDbAssemblyPath, connection);

            Console.WriteLine("Generating script to update(from v1 to v2) SQLiteSampleDB from the corresponding assembly files...");
            dmDataLoss.GeneratePublishScript(dbAssemblyV2, dbAssembly, s_sqliteGeneratedPublishFromV1ToV2ScriptPath);
            dmDataLoss.GeneratePublishScript(s_sqliteDbV2SqlDefAssemblyPath, s_sqliteDbAssemblyPath, s_sqliteGeneratedPublishFromV1ToV2SqlDefScriptPath);
            Console.WriteLine("Updating(from v1 to v2) existing SQLiteSampleDB from dbAssembly v2 file...");
            dmDataLoss.PublishDatabase(s_sqliteDbV2AssemblyPath, connection);

            Console.WriteLine("Generating script to update(rollback from v2 to v1) existing SQLiteSampleDB from dbAssembly file...");
            dmDataLoss.GeneratePublishScript(s_sqliteDbAssemblyPath, connection, s_sqliteGeneratedPublishFromV2ToV1ScriptPath);
            Console.WriteLine("Updating(rollback from v2 to v1) SQLiteSampleDB using previously generated script...");
            connection.Execute(File.ReadAllText(s_sqliteGeneratedPublishFromV2ToV1ScriptPath));

            Console.WriteLine("Unregistering(=deleting DNDBT system information from DB) SQLiteSampleDB...");
            deployManager.UnregisterAsDNDBT(connection);
            Console.WriteLine("Generating definition from existing unregistered SQLiteSampleDB...");
            deployManager.GenerateDefinition(connection, s_sqliteGeneratedDefinitionFromUnregisteredDir);

            Console.WriteLine("Generating NoDNDBTInfo script to update(from v1 to v2) SQLiteSampleDB from the corresponding assembly files...");
            dmDataLoss.GenerateNoDNDBTInfoPublishScript(dbAssemblyV2, dbAssembly, s_sqliteGeneratedNoDNDBTInfoPublishFromV1ToV2ScriptPath);
            Console.WriteLine("Updating(from v1 to v2) SQLiteSampleDB using previously generated NoDNDBTInfo script...");
            connection.Execute(File.ReadAllText(s_sqliteGeneratedNoDNDBTInfoPublishFromV1ToV2ScriptPath));

            Console.WriteLine("Generating NoDNDBTInfo script to update(rollback from v2 to v1) SQLiteSampleDB from the corresponding assembly files...");
            dmDataLoss.GenerateNoDNDBTInfoPublishScript(dbAssembly, dbAssemblyV2, s_sqliteGeneratedNoDNDBTInfoPublishFromV2ToV1ScriptPath);
            Console.WriteLine("Updating(rollback from v2 to v1) SQLiteSampleDB using previously generated NoDNDBTInfo script...");
            connection.Execute(File.ReadAllText(s_sqliteGeneratedNoDNDBTInfoPublishFromV2ToV1ScriptPath));

            Console.WriteLine("Registering(=generating and adding new DNDBT system information to DB) SQLiteSampleDB...");
            deployManager.RegisterAsDNDBT(connection);
            Console.WriteLine("Generating definition from existing registered SQLiteSampleDB...");
            deployManager.GenerateDefinition(connection, s_sqliteGeneratedDefinitionFromRegisteredDir);

            Console.WriteLine("Newly created DNDBT system information(by registration) has different IDs for all objects");
            Console.WriteLine("Generating script to update(recreate all objects) existing SQLiteSampleDB from dbAssembly file...");
            dmDataLoss.GeneratePublishScript(s_sqliteDbAssemblyPath, connection, s_sqliteGeneratedPublishRecreateScriptPath);
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
