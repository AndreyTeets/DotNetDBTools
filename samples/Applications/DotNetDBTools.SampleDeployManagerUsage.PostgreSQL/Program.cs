﻿using System;
using System.IO;
using System.Reflection;
using Dapper;
using DotNetDBTools.Deploy;
using DotNetDBTools.EventsLogger;
using Npgsql;

namespace DotNetDBTools.SampleDeployManagerUsage.PostgreSQL
{
    public static class Program
    {
        private const string PostgreSQLServerPassword = "Strong(!)Passw0rd";
        private const string PostgreSQLServerHostPort = "5007";
        private const string AgnosticDatabaseName = "AgnosticSampleDB";
        private const string PostgreSQLDatabaseName = "PostgreSQLSampleDB";

        private const string RepoRoot = "../../../../../..";
        private static readonly string s_samplesOutputDir = $"{RepoRoot}/SamplesOutput";

        private static readonly string s_agnosticDbAssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDB.Agnostic.dll";
        private static readonly string s_agnosticDbV2AssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDBv2.Agnostic.dll";
        private static readonly string s_agnosticConnectionString = $"Host=127.0.0.1;Port={PostgreSQLServerHostPort};Database={AgnosticDatabaseName};Username=postgres;Password={PostgreSQLServerPassword}";

        private static readonly string s_postgresqlDbAssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDB.PostgreSQL.dll";
        private static readonly string s_postgresqlDbV2AssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDBv2.PostgreSQL.dll";
        private static readonly string s_postgresqlConnectionString = $"Host=127.0.0.1;Port={PostgreSQLServerHostPort};Database={PostgreSQLDatabaseName};Username=postgres;Password={PostgreSQLServerPassword}";

        private static readonly string s_agnosticGeneratedPublishToEmptyScriptPath = $"{s_samplesOutputDir}/postgresql_generated/AgnosticGeneratedPublishToEmptyScript.sql";
        private static readonly string s_agnosticGeneratedPublishToExistingScriptPath = $"{s_samplesOutputDir}/postgresql_generated/AgnosticGeneratedPublishToExistingScript.sql";
        private static readonly string s_agnosticGeneratedPublishFromV1ToV2ScriptPath = $"{s_samplesOutputDir}/postgresql_generated/AgnosticGeneratedPublishFromV1ToV2Script.sql";
        private static readonly string s_agnosticGeneratedNoDNDBTInfoPublishFromV1ToV2ScriptPath = $"{s_samplesOutputDir}/postgresql_generated/AgnosticGeneratedNoDNDBTInfoPublishFromV1ToV2Script.sql";
        private static readonly string s_agnosticGeneratedPublishFromV2ToV1ScriptPath = $"{s_samplesOutputDir}/postgresql_generated/AgnosticGeneratedPublishFromV2ToV1Script.sql";
        private static readonly string s_agnosticGeneratedNoDNDBTInfoPublishFromV2ToV1ScriptPath = $"{s_samplesOutputDir}/postgresql_generated/AgnosticGeneratedNoDNDBTInfoPublishFromV2ToV1Script.sql";
        private static readonly string s_agnosticGeneratedDefinitionFromUnregisteredDir = $"{s_samplesOutputDir}/postgresql_generated/AgnosticGeneratedDefinitionFromUnregistered";
        private static readonly string s_agnosticGeneratedDefinitionFromRegisteredDir = $"{s_samplesOutputDir}/postgresql_generated/AgnosticGeneratedDefinitionFromRegistered";
        private static readonly string s_agnosticGeneratedPublishRecreateScriptPath = $"{s_samplesOutputDir}/postgresql_generated/AgnosticGeneratedPublishRecreateScript.sql";

        private static readonly string s_postgresqlGeneratedPublishToEmptyScriptPath = $"{s_samplesOutputDir}/postgresql_generated/PostgreSQLGeneratedPublishToEmptyScript.sql";
        private static readonly string s_postgresqlGeneratedPublishToExistingScriptPath = $"{s_samplesOutputDir}/postgresql_generated/PostgreSQLGeneratedPublishToExistingScript.sql";
        private static readonly string s_postgresqlGeneratedPublishFromV1ToV2ScriptPath = $"{s_samplesOutputDir}/postgresql_generated/PostgreSQLGeneratedPublishFromV1ToV2Script.sql";
        private static readonly string s_postgresqlGeneratedNoDNDBTInfoPublishFromV1ToV2ScriptPath = $"{s_samplesOutputDir}/postgresql_generated/PostgreSQLGeneratedNoDNDBTInfoPublishFromV1ToV2Script.sql";
        private static readonly string s_postgresqlGeneratedPublishFromV2ToV1ScriptPath = $"{s_samplesOutputDir}/postgresql_generated/PostgreSQLGeneratedPublishFromV2ToV1Script.sql";
        private static readonly string s_postgresqlGeneratedNoDNDBTInfoPublishFromV2ToV1ScriptPath = $"{s_samplesOutputDir}/postgresql_generated/PostgreSQLGeneratedNoDNDBTInfoPublishFromV2ToV1Script.sql";
        private static readonly string s_postgresqlGeneratedDefinitionFromUnregisteredDir = $"{s_samplesOutputDir}/postgresql_generated/PostgreSQLGeneratedDefinitionFromUnregistered";
        private static readonly string s_postgresqlGeneratedDefinitionFromRegisteredDir = $"{s_samplesOutputDir}/postgresql_generated/PostgreSQLGeneratedDefinitionFromRegistered";
        private static readonly string s_postgresqlGeneratedPublishRecreateScriptPath = $"{s_samplesOutputDir}/postgresql_generated/PostgreSQLGeneratedPublishRecreateScript.sql";

        public static void Main()
        {
            RunAgnosticSampleDBDeployExamples();
            RunPostgreSQLSampleDBDeployExamples();
        }

        private static void RunAgnosticSampleDBDeployExamples()
        {
            DropDatabaseIfExists(s_agnosticConnectionString);
            CreateDatabase(s_agnosticConnectionString);

            Assembly dbAssembly = Assembly.Load(File.ReadAllBytes(s_agnosticDbAssemblyPath));
            Assembly dbAssemblyV2 = Assembly.Load(File.ReadAllBytes(s_agnosticDbV2AssemblyPath));

            IDeployManager deployManager = new PostgreSQLDeployManager(new DeployOptions());
            deployManager.Events.EventFired += DeployManagerEventsLogger.LogEvent;
            IDeployManager dmDataLoss = new PostgreSQLDeployManager(new DeployOptions { AllowDataLoss = true });
            dmDataLoss.Events.EventFired += DeployManagerEventsLogger.LogEvent;
            using NpgsqlConnection connection = new(s_agnosticConnectionString);

            Console.WriteLine("Registering empty AgnosticSampleDB as DNDBT to make other actions with it possible...");
            deployManager.RegisterAsDNDBT(connection);

            Console.WriteLine("Generating script to create new AgnosticSampleDB from dbAssembly file...");
            deployManager.GeneratePublishScript(s_agnosticDbAssemblyPath, connection, s_agnosticGeneratedPublishToEmptyScriptPath);
            Console.WriteLine("Creating new AgnosticSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_agnosticDbAssemblyPath, connection);

            Console.WriteLine("Generating script to update(no changes) existing AgnosticSampleDB from dbAssembly file...");
            deployManager.GeneratePublishScript(s_agnosticDbAssemblyPath, connection, s_agnosticGeneratedPublishToExistingScriptPath);
            Console.WriteLine("Updating(no changes) existing AgnosticSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_agnosticDbAssemblyPath, connection);

            Console.WriteLine("Generating script to update(from v1 to v2) AgnosticSampleDB from the corresponding assembly files...");
            dmDataLoss.GeneratePublishScript(dbAssemblyV2, dbAssembly, s_agnosticGeneratedPublishFromV1ToV2ScriptPath);
            Console.WriteLine("Updating(from v1 to v2) existing AgnosticSampleDB from dbAssembly v2 file...");
            dmDataLoss.PublishDatabase(s_agnosticDbV2AssemblyPath, connection);

            Console.WriteLine("Generating script to update(rollback from v2 to v1) existing AgnosticSampleDB from dbAssembly file...");
            dmDataLoss.GeneratePublishScript(s_agnosticDbAssemblyPath, connection, s_agnosticGeneratedPublishFromV2ToV1ScriptPath);
            Console.WriteLine("Updating(rollback from v2 to v1) AgnosticSampleDB using previously generated script...");
            connection.Execute(File.ReadAllText(s_agnosticGeneratedPublishFromV2ToV1ScriptPath));

            Console.WriteLine("Unregistering(=deleting DNDBT system information from DB) AgnosticSampleDB...");
            deployManager.UnregisterAsDNDBT(connection);
            Console.WriteLine("Generating definition from existing unregistered AgnosticSampleDB...");
            deployManager.GenerateDefinition(connection, s_agnosticGeneratedDefinitionFromUnregisteredDir);

            Console.WriteLine("Generating NoDNDBTInfo script to update(from v1 to v2) AgnosticSampleDB from the corresponding assembly files...");
            dmDataLoss.GenerateNoDNDBTInfoPublishScript(dbAssemblyV2, dbAssembly, s_agnosticGeneratedNoDNDBTInfoPublishFromV1ToV2ScriptPath);
            Console.WriteLine("Updating(from v1 to v2) AgnosticSampleDB using previously generated NoDNDBTInfo script...");
            connection.Execute(File.ReadAllText(s_agnosticGeneratedNoDNDBTInfoPublishFromV1ToV2ScriptPath));

            Console.WriteLine("Generating NoDNDBTInfo script to update(rollback from v2 to v1) AgnosticSampleDB from the corresponding assembly files...");
            dmDataLoss.GenerateNoDNDBTInfoPublishScript(dbAssembly, dbAssemblyV2, s_agnosticGeneratedNoDNDBTInfoPublishFromV2ToV1ScriptPath);
            Console.WriteLine("Updating(rollback from v2 to v1) AgnosticSampleDB using previously generated NoDNDBTInfo script...");
            connection.Execute(File.ReadAllText(s_agnosticGeneratedNoDNDBTInfoPublishFromV2ToV1ScriptPath));

            Console.WriteLine("Registering(=generating and adding new DNDBT system information to DB) AgnosticSampleDB...");
            deployManager.RegisterAsDNDBT(connection);
            Console.WriteLine("Generating definition from existing registered AgnosticSampleDB...");
            deployManager.GenerateDefinition(connection, s_agnosticGeneratedDefinitionFromRegisteredDir);

            Console.WriteLine("Newly created DNDBT system information(by registration) has different IDs for all objects");
            Console.WriteLine("Generating script to update(recreate all objects) existing AgnosticSampleDB from dbAssembly file...");
            dmDataLoss.GeneratePublishScript(s_agnosticDbAssemblyPath, connection, s_agnosticGeneratedPublishRecreateScriptPath);
            Console.WriteLine("Updating(recreate all objects) existing AgnosticSampleDB from dbAssembly file...");
            dmDataLoss.PublishDatabase(s_agnosticDbAssemblyPath, connection);
        }

        private static void RunPostgreSQLSampleDBDeployExamples()
        {
            DropDatabaseIfExists(s_postgresqlConnectionString);
            CreateDatabase(s_postgresqlConnectionString);

            Assembly dbAssembly = Assembly.Load(File.ReadAllBytes(s_postgresqlDbAssemblyPath));
            Assembly dbAssemblyV2 = Assembly.Load(File.ReadAllBytes(s_postgresqlDbV2AssemblyPath));

            IDeployManager deployManager = new PostgreSQLDeployManager(new DeployOptions());
            deployManager.Events.EventFired += DeployManagerEventsLogger.LogEvent;
            IDeployManager dmDataLoss = new PostgreSQLDeployManager(new DeployOptions { AllowDataLoss = true });
            dmDataLoss.Events.EventFired += DeployManagerEventsLogger.LogEvent;
            using NpgsqlConnection connection = new(s_postgresqlConnectionString);

            Console.WriteLine("Registering empty AgnosticSampleDB as DNDBT to make other actions with it possible...");
            deployManager.RegisterAsDNDBT(connection);

            Console.WriteLine("Generating script to create new PostgreSQLSampleDB from dbAssembly file...");
            deployManager.GeneratePublishScript(s_postgresqlDbAssemblyPath, connection, s_postgresqlGeneratedPublishToEmptyScriptPath);
            Console.WriteLine("Creating new PostgreSQLSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_postgresqlDbAssemblyPath, connection);

            Console.WriteLine("Generating script to update(no changes) existing PostgreSQLSampleDB from dbAssembly file...");
            deployManager.GeneratePublishScript(s_postgresqlDbAssemblyPath, connection, s_postgresqlGeneratedPublishToExistingScriptPath);
            Console.WriteLine("Updating(no changes) existing PostgreSQLSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_postgresqlDbAssemblyPath, connection);

            Console.WriteLine("Generating script to update(from v1 to v2) PostgreSQLSampleDB from the corresponding assembly files...");
            dmDataLoss.GeneratePublishScript(dbAssemblyV2, dbAssembly, s_postgresqlGeneratedPublishFromV1ToV2ScriptPath);
            Console.WriteLine("Updating(from v1 to v2) existing PostgreSQLSampleDB from dbAssembly v2 file...");
            dmDataLoss.PublishDatabase(s_postgresqlDbV2AssemblyPath, connection);

            Console.WriteLine("Generating script to update(rollback from v2 to v1) existing PostgreSQLSampleDB from dbAssembly file...");
            dmDataLoss.GeneratePublishScript(s_postgresqlDbAssemblyPath, connection, s_postgresqlGeneratedPublishFromV2ToV1ScriptPath);
            Console.WriteLine("Updating(rollback from v2 to v1) PostgreSQLSampleDB using previously generated script...");
            connection.Execute(File.ReadAllText(s_postgresqlGeneratedPublishFromV2ToV1ScriptPath));

            Console.WriteLine("Unregistering(=deleting DNDBT system information from DB) PostgreSQLSampleDB...");
            deployManager.UnregisterAsDNDBT(connection);
            Console.WriteLine("Generating definition from existing unregistered PostgreSQLSampleDB...");
            deployManager.GenerateDefinition(connection, s_postgresqlGeneratedDefinitionFromUnregisteredDir);

            Console.WriteLine("Generating NoDNDBTInfo script to update(from v1 to v2) PostgreSQLSampleDB from the corresponding assembly files...");
            dmDataLoss.GenerateNoDNDBTInfoPublishScript(dbAssemblyV2, dbAssembly, s_postgresqlGeneratedNoDNDBTInfoPublishFromV1ToV2ScriptPath);
            Console.WriteLine("Updating(from v1 to v2) PostgreSQLSampleDB using previously generated NoDNDBTInfo script...");
            connection.Execute(File.ReadAllText(s_postgresqlGeneratedNoDNDBTInfoPublishFromV1ToV2ScriptPath));

            Console.WriteLine("Generating NoDNDBTInfo script to update(rollback from v2 to v1) PostgreSQLSampleDB from the corresponding assembly files...");
            dmDataLoss.GenerateNoDNDBTInfoPublishScript(dbAssembly, dbAssemblyV2, s_postgresqlGeneratedNoDNDBTInfoPublishFromV2ToV1ScriptPath);
            Console.WriteLine("Updating(rollback from v2 to v1) PostgreSQLSampleDB using previously generated NoDNDBTInfo script...");
            connection.Execute(File.ReadAllText(s_postgresqlGeneratedNoDNDBTInfoPublishFromV2ToV1ScriptPath));

            Console.WriteLine("Registering(=generating and adding new DNDBT system information to DB) PostgreSQLSampleDB...");
            deployManager.RegisterAsDNDBT(connection);
            Console.WriteLine("Generating definition from existing registered PostgreSQLSampleDB...");
            deployManager.GenerateDefinition(connection, s_postgresqlGeneratedDefinitionFromRegisteredDir);

            Console.WriteLine("Newly created DNDBT system information(by registration) has different IDs for all objects");
            Console.WriteLine("Generating script to update(recreate all objects) existing AgnosticSampleDB from dbAssembly file...");
            dmDataLoss.GeneratePublishScript(s_postgresqlDbAssemblyPath, connection, s_postgresqlGeneratedPublishRecreateScriptPath);
            Console.WriteLine("Updating(recreate all objects) existing AgnosticSampleDB from dbAssembly file...");
            dmDataLoss.PublishDatabase(s_postgresqlDbAssemblyPath, connection);
        }

        private static void CreateDatabase(string connectionString)
        {
            NpgsqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.Database;
            connectionStringBuilder.Database = string.Empty;
            string connectionStringWithoutDb = connectionStringBuilder.ConnectionString;

            using NpgsqlConnection connection = new(connectionStringWithoutDb);
            connection.Execute(
$@"CREATE DATABASE ""{databaseName}"";");
        }

        private static void DropDatabaseIfExists(string connectionString)
        {
            NpgsqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.Database;
            connectionStringBuilder.Database = string.Empty;
            string connectionStringWithoutDb = connectionStringBuilder.ConnectionString;

            using NpgsqlConnection connection = new(connectionStringWithoutDb);
            connection.Execute(
$@"SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = '{databaseName}';
DROP DATABASE IF EXISTS ""{databaseName}"";");
        }
    }
}