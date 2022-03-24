using System;
using System.IO;
using System.Reflection;
using Dapper;
using DotNetDBTools.Deploy;
using DotNetDBTools.EventsLogger;
using MySqlConnector;

namespace DotNetDBTools.SampleDeployManagerUsage.MySQL
{
    public static class Program
    {
        private const string MySQLServerPassword = "Strong(!)Passw0rd";
        private const string MySQLServerHostPort = "5006";
        private const string AgnosticDatabaseName = "AgnosticSampleDB";
        private const string MySQLDatabaseName = "MySQLSampleDB";

        private const string RepoRoot = "../../../../../..";
        private static readonly string s_samplesOutputDir = $"{RepoRoot}/SamplesOutput";

        private static readonly string s_agnosticDbAssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDB.Agnostic.dll";
        private static readonly string s_agnosticDbV2AssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDBv2.Agnostic.dll";
        private static readonly string s_agnosticConnectionString = $"Host=localhost;Port={MySQLServerHostPort};Database={AgnosticDatabaseName};Username=root;Password={MySQLServerPassword}";

        private static readonly string s_mysqlDbAssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDB.MySQL.dll";
        private static readonly string s_mysqlDbV2AssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDBv2.MySQL.dll";
        private static readonly string s_mysqlConnectionString = $"Host=localhost;Port={MySQLServerHostPort};Database={MySQLDatabaseName};Username=root;Password={MySQLServerPassword}";

        private static readonly string s_agnosticGeneratedPublishToEmptyScriptPath = $"{s_samplesOutputDir}/mysql_generated/AgnosticGeneratedPublishToEmptyScript.sql";
        private static readonly string s_agnosticGeneratedPublishToExistingScriptPath = $"{s_samplesOutputDir}/mysql_generated/AgnosticGeneratedPublishToExistingScript.sql";
        private static readonly string s_agnosticGeneratedPublishFromV1ToV2ScriptPath = $"{s_samplesOutputDir}/mysql_generated/AgnosticGeneratedPublishFromV1ToV2Script.sql";
        private static readonly string s_agnosticGeneratedNoDNDBTInfoPublishFromV1ToV2ScriptPath = $"{s_samplesOutputDir}/mysql_generated/AgnosticGeneratedNoDNDBTInfoPublishFromV1ToV2Script.sql";
        private static readonly string s_agnosticGeneratedPublishFromV2ToV1ScriptPath = $"{s_samplesOutputDir}/mysql_generated/AgnosticGeneratedPublishFromV2ToV1Script.sql";
        private static readonly string s_agnosticGeneratedNoDNDBTInfoPublishFromV2ToV1ScriptPath = $"{s_samplesOutputDir}/mysql_generated/AgnosticGeneratedNoDNDBTInfoPublishFromV2ToV1Script.sql";
        private static readonly string s_agnosticGeneratedDefinitionFromUnregisteredDir = $"{s_samplesOutputDir}/mysql_generated/AgnosticGeneratedDefinitionFromUnregistered";
        private static readonly string s_agnosticGeneratedDefinitionFromRegisteredDir = $"{s_samplesOutputDir}/mysql_generated/AgnosticGeneratedDefinitionFromRegistered";
        private static readonly string s_agnosticGeneratedPublishRecreateScriptPath = $"{s_samplesOutputDir}/mysql_generated/AgnosticGeneratedPublishRecreateScript.sql";

        private static readonly string s_mysqlGeneratedPublishToEmptyScriptPath = $"{s_samplesOutputDir}/mysql_generated/MySQLGeneratedPublishToEmptyScript.sql";
        private static readonly string s_mysqlGeneratedPublishToExistingScriptPath = $"{s_samplesOutputDir}/mysql_generated/MySQLGeneratedPublishToExistingScript.sql";
        private static readonly string s_mysqlGeneratedPublishFromV1ToV2ScriptPath = $"{s_samplesOutputDir}/mysql_generated/MySQLGeneratedPublishFromV1ToV2Script.sql";
        private static readonly string s_mysqlGeneratedNoDNDBTInfoPublishFromV1ToV2ScriptPath = $"{s_samplesOutputDir}/mysql_generated/MySQLGeneratedNoDNDBTInfoPublishFromV1ToV2Script.sql";
        private static readonly string s_mysqlGeneratedPublishFromV2ToV1ScriptPath = $"{s_samplesOutputDir}/mysql_generated/MySQLGeneratedPublishFromV2ToV1Script.sql";
        private static readonly string s_mysqlGeneratedNoDNDBTInfoPublishFromV2ToV1ScriptPath = $"{s_samplesOutputDir}/mysql_generated/MySQLGeneratedNoDNDBTInfoPublishFromV2ToV1Script.sql";
        private static readonly string s_mysqlGeneratedDefinitionFromUnregisteredDir = $"{s_samplesOutputDir}/mysql_generated/MySQLGeneratedDefinitionFromUnregistered";
        private static readonly string s_mysqlGeneratedDefinitionFromRegisteredDir = $"{s_samplesOutputDir}/mysql_generated/MySQLGeneratedDefinitionFromRegistered";
        private static readonly string s_mysqlGeneratedPublishRecreateScriptPath = $"{s_samplesOutputDir}/mysql_generated/MySQLGeneratedPublishRecreateScript.sql";

        public static void Main()
        {
            RunAgnosticSampleDBDeployExamples();
            RunMySQLSampleDBDeployExamples();
        }

        private static void RunAgnosticSampleDBDeployExamples()
        {
            DropDatabaseIfExists(s_agnosticConnectionString);
            CreateDatabase(s_agnosticConnectionString);

            Assembly dbAssembly = Assembly.Load(File.ReadAllBytes(s_agnosticDbAssemblyPath));
            Assembly dbAssemblyV2 = Assembly.Load(File.ReadAllBytes(s_agnosticDbV2AssemblyPath));

            IDeployManager deployManager = new MySQLDeployManager(new DeployOptions());
            deployManager.Events.EventFired += DeployManagerEventsLogger.LogEvent;
            IDeployManager dmDataLoss = new MySQLDeployManager(new DeployOptions { AllowDataLoss = true });
            dmDataLoss.Events.EventFired += DeployManagerEventsLogger.LogEvent;
            using MySqlConnection connection = new(s_agnosticConnectionString);

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

        private static void RunMySQLSampleDBDeployExamples()
        {
            DropDatabaseIfExists(s_mysqlConnectionString);
            CreateDatabase(s_mysqlConnectionString);

            Assembly dbAssembly = Assembly.Load(File.ReadAllBytes(s_mysqlDbAssemblyPath));
            Assembly dbAssemblyV2 = Assembly.Load(File.ReadAllBytes(s_mysqlDbV2AssemblyPath));

            IDeployManager deployManager = new MySQLDeployManager(new DeployOptions());
            deployManager.Events.EventFired += DeployManagerEventsLogger.LogEvent;
            IDeployManager dmDataLoss = new MySQLDeployManager(new DeployOptions { AllowDataLoss = true });
            dmDataLoss.Events.EventFired += DeployManagerEventsLogger.LogEvent;
            using MySqlConnection connection = new(s_mysqlConnectionString);

            Console.WriteLine("Registering empty AgnosticSampleDB as DNDBT to make other actions with it possible...");
            deployManager.RegisterAsDNDBT(connection);

            Console.WriteLine("Generating script to create new MySQLSampleDB from dbAssembly file...");
            deployManager.GeneratePublishScript(s_mysqlDbAssemblyPath, connection, s_mysqlGeneratedPublishToEmptyScriptPath);
            Console.WriteLine("Creating new MySQLSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_mysqlDbAssemblyPath, connection);

            Console.WriteLine("Generating script to update(no changes) existing MySQLSampleDB from dbAssembly file...");
            deployManager.GeneratePublishScript(s_mysqlDbAssemblyPath, connection, s_mysqlGeneratedPublishToExistingScriptPath);
            Console.WriteLine("Updating(no changes) existing MySQLSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_mysqlDbAssemblyPath, connection);

            Console.WriteLine("Generating script to update(from v1 to v2) MySQLSampleDB from the corresponding assembly files...");
            dmDataLoss.GeneratePublishScript(dbAssemblyV2, dbAssembly, s_mysqlGeneratedPublishFromV1ToV2ScriptPath);
            Console.WriteLine("Updating(from v1 to v2) existing MySQLSampleDB from dbAssembly v2 file...");
            dmDataLoss.PublishDatabase(s_mysqlDbV2AssemblyPath, connection);

            Console.WriteLine("Generating script to update(rollback from v2 to v1) existing MySQLSampleDB from dbAssembly file...");
            dmDataLoss.GeneratePublishScript(s_mysqlDbAssemblyPath, connection, s_mysqlGeneratedPublishFromV2ToV1ScriptPath);
            Console.WriteLine("Updating(rollback from v2 to v1) MySQLSampleDB using previously generated script...");
            connection.Execute(File.ReadAllText(s_mysqlGeneratedPublishFromV2ToV1ScriptPath));

            Console.WriteLine("Unregistering(=deleting DNDBT system information from DB) MySQLSampleDB...");
            deployManager.UnregisterAsDNDBT(connection);
            Console.WriteLine("Generating definition from existing unregistered MySQLSampleDB...");
            deployManager.GenerateDefinition(connection, s_mysqlGeneratedDefinitionFromUnregisteredDir);

            Console.WriteLine("Generating NoDNDBTInfo script to update(from v1 to v2) MySQLSampleDB from the corresponding assembly files...");
            dmDataLoss.GenerateNoDNDBTInfoPublishScript(dbAssemblyV2, dbAssembly, s_mysqlGeneratedNoDNDBTInfoPublishFromV1ToV2ScriptPath);
            Console.WriteLine("Updating(from v1 to v2) MySQLSampleDB using previously generated NoDNDBTInfo script...");
            connection.Execute(File.ReadAllText(s_mysqlGeneratedNoDNDBTInfoPublishFromV1ToV2ScriptPath));

            Console.WriteLine("Generating NoDNDBTInfo script to update(rollback from v2 to v1) MySQLSampleDB from the corresponding assembly files...");
            dmDataLoss.GenerateNoDNDBTInfoPublishScript(dbAssembly, dbAssemblyV2, s_mysqlGeneratedNoDNDBTInfoPublishFromV2ToV1ScriptPath);
            Console.WriteLine("Updating(rollback from v2 to v1) MySQLSampleDB using previously generated NoDNDBTInfo script...");
            connection.Execute(File.ReadAllText(s_mysqlGeneratedNoDNDBTInfoPublishFromV2ToV1ScriptPath));

            Console.WriteLine("Registering(=generating and adding new DNDBT system information to DB) MySQLSampleDB...");
            deployManager.RegisterAsDNDBT(connection);
            Console.WriteLine("Generating definition from existing registered MySQLSampleDB...");
            deployManager.GenerateDefinition(connection, s_mysqlGeneratedDefinitionFromRegisteredDir);

            Console.WriteLine("Newly created DNDBT system information(by registration) has different IDs for all objects");
            Console.WriteLine("Generating script to update(recreate all objects) existing AgnosticSampleDB from dbAssembly file...");
            dmDataLoss.GeneratePublishScript(s_mysqlDbAssemblyPath, connection, s_mysqlGeneratedPublishRecreateScriptPath);
            Console.WriteLine("Updating(recreate all objects) existing AgnosticSampleDB from dbAssembly file...");
            dmDataLoss.PublishDatabase(s_mysqlDbAssemblyPath, connection);
        }

        private static void CreateDatabase(string connectionString)
        {
            MySqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.Database;
            connectionStringBuilder.Database = string.Empty;
            string connectionStringWithoutDb = connectionStringBuilder.ConnectionString;

            using MySqlConnection connection = new(connectionStringWithoutDb);
            connection.Execute(
$@"CREATE DATABASE `{databaseName}`;");
        }

        private static void DropDatabaseIfExists(string connectionString)
        {
            MySqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.Database;
            connectionStringBuilder.Database = string.Empty;
            string connectionStringWithoutDb = connectionStringBuilder.ConnectionString;

            using MySqlConnection connection = new(connectionStringWithoutDb);
            connection.Execute(
$@"DROP DATABASE IF EXISTS `{databaseName}`;");
        }
    }
}
