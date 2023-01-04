using System;
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
        private const string PostgreSQLDatabaseName = "PostgreSQLSampleDB_BusinessLogicOnlyApp";

        private const string RepoRoot = "../../../../../..";
        private static readonly string s_samplesOutputDir = $"{RepoRoot}/SamplesOutput";
        private static readonly string s_generatedOutputDir = $"{s_samplesOutputDir}/postgresql_generated";

        private static readonly string s_postgresqlDbAssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDB.PostgreSQL.dll";
        private static readonly string s_postgresqlDbV2AssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDBv2.PostgreSQL.dll";
        private static readonly string s_postgresqlDbV2SqlDefAssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDBv2SqlDef.PostgreSQL.dll";
        private static readonly string s_postgresqlConnectionString = $"Host=127.0.0.1;Port={PostgreSQLServerHostPort};Database={PostgreSQLDatabaseName};Username=postgres;Password={PostgreSQLServerPassword}";

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
            RunPostgreSQLSampleDBDeployExamples();
        }

        private static void RunPostgreSQLSampleDBDeployExamples()
        {
            DropDatabaseIfExists(s_postgresqlConnectionString);
            CreateDatabase(s_postgresqlConnectionString);
            Directory.CreateDirectory(s_generatedOutputDir);

            Assembly dbAssembly = Assembly.Load(File.ReadAllBytes(s_postgresqlDbAssemblyPath));
            Assembly dbAssemblyV2 = Assembly.Load(File.ReadAllBytes(s_postgresqlDbV2AssemblyPath));

            IDeployManager deployManager = new PostgreSQLDeployManager(new DeployOptions());
            deployManager.Events.EventFired += DeployManagerEventsLogger.LogEvent;
            IDeployManager dmDataLoss = new PostgreSQLDeployManager(new DeployOptions { AllowDataLoss = true });
            dmDataLoss.Events.EventFired += DeployManagerEventsLogger.LogEvent;
            using NpgsqlConnection connection = new(s_postgresqlConnectionString);

            Console.WriteLine("Registering empty PostgreSQLSampleDB as DNDBT to make other actions with it possible...");
            deployManager.RegisterAsDNDBT(connection);

            Console.WriteLine("Generating script to create new PostgreSQLSampleDB from dbAssembly file...");
            File.WriteAllText(s_publishToEmptyScriptPath, deployManager.GeneratePublishScript(s_postgresqlDbAssemblyPath, connection));
            Console.WriteLine("Creating new PostgreSQLSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_postgresqlDbAssemblyPath, connection);

            Console.WriteLine("Generating script to update(no changes) existing PostgreSQLSampleDB from dbAssembly file...");
            File.WriteAllText(s_publishToExistingScriptPath, deployManager.GeneratePublishScript(s_postgresqlDbAssemblyPath, connection));
            Console.WriteLine("Updating(no changes) existing PostgreSQLSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_postgresqlDbAssemblyPath, connection);

            Console.WriteLine("Generating script to update(from v1 to v2) PostgreSQLSampleDB from the corresponding assembly files...");
            File.WriteAllText(s_publishFromV1ToV2ScriptPath, dmDataLoss.GeneratePublishScript(dbAssemblyV2, dbAssembly));
            File.WriteAllText(s_publishFromV1ToV2SqlDefScriptPath, dmDataLoss.GeneratePublishScript(s_postgresqlDbV2SqlDefAssemblyPath, s_postgresqlDbAssemblyPath));
            Console.WriteLine("Updating(from v1 to v2) existing PostgreSQLSampleDB from dbAssembly v2 file...");
            dmDataLoss.PublishDatabase(s_postgresqlDbV2AssemblyPath, connection);

            Console.WriteLine("Generating script to update(rollback from v2 to v1) existing PostgreSQLSampleDB from dbAssembly file...");
            File.WriteAllText(s_publishFromV2ToV1ScriptPath, dmDataLoss.GeneratePublishScript(s_postgresqlDbAssemblyPath, connection));
            Console.WriteLine("Updating(rollback from v2 to v1) PostgreSQLSampleDB using previously generated script...");
            connection.Execute(File.ReadAllText(s_publishFromV2ToV1ScriptPath));

            Console.WriteLine("Unregistering(=deleting DNDBT system information from DB) PostgreSQLSampleDB...");
            deployManager.UnregisterAsDNDBT(connection);
            Console.WriteLine("Generating definition from existing unregistered PostgreSQLSampleDB...");
            deployManager.GenerateDefinition(connection, s_definitionFromUnregisteredDir);

            Console.WriteLine("Generating NoDNDBTInfo script to update(from v1 to v2) PostgreSQLSampleDB from the corresponding assembly files...");
            File.WriteAllText(s_noDNDBTInfoPublishFromV1ToV2ScriptPath, dmDataLoss.GenerateNoDNDBTInfoPublishScript(dbAssemblyV2, dbAssembly));
            Console.WriteLine("Updating(from v1 to v2) PostgreSQLSampleDB using previously generated NoDNDBTInfo script...");
            connection.Execute(File.ReadAllText(s_noDNDBTInfoPublishFromV1ToV2ScriptPath));

            Console.WriteLine("Generating NoDNDBTInfo script to update(rollback from v2 to v1) PostgreSQLSampleDB from the corresponding assembly files...");
            File.WriteAllText(s_noDNDBTInfoPublishFromV2ToV1ScriptPath, dmDataLoss.GenerateNoDNDBTInfoPublishScript(dbAssembly, dbAssemblyV2));
            Console.WriteLine("Updating(rollback from v2 to v1) PostgreSQLSampleDB using previously generated NoDNDBTInfo script...");
            connection.Execute(File.ReadAllText(s_noDNDBTInfoPublishFromV2ToV1ScriptPath));

            Console.WriteLine("Registering(=generating and adding new DNDBT system information to DB) PostgreSQLSampleDB...");
            deployManager.RegisterAsDNDBT(connection);
            Console.WriteLine("Generating definition from existing registered PostgreSQLSampleDB...");
            deployManager.GenerateDefinition(connection, s_definitionFromRegisteredDir);

            Console.WriteLine("Newly created DNDBT system information(by registration) has different IDs for all objects");
            Console.WriteLine("Generating script to update(recreate all objects) existing PostgreSQLSampleDB from dbAssembly file...");
            File.WriteAllText(s_publishRecreateScriptPath, dmDataLoss.GeneratePublishScript(s_postgresqlDbAssemblyPath, connection));
            Console.WriteLine("Updating(recreate all objects) existing PostgreSQLSampleDB from dbAssembly file...");
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
