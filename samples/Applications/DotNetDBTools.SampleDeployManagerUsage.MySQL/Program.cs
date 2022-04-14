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
        private const string MySQLDatabaseName = "MySQLSampleDB_BusinessLogicOnlyApp";

        private const string RepoRoot = "../../../../../..";
        private static readonly string s_samplesOutputDir = $"{RepoRoot}/SamplesOutput";

        private static readonly string s_mysqlDbAssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDB.MySQL.dll";
        private static readonly string s_mysqlDbV2AssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDBv2.MySQL.dll";
        private static readonly string s_mysqlConnectionString = $"Host=127.0.0.1;Port={MySQLServerHostPort};Database={MySQLDatabaseName};Username=root;Password={MySQLServerPassword}";

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
            RunMySQLSampleDBDeployExamples();
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

            Console.WriteLine("Registering empty MySQLSampleDB as DNDBT to make other actions with it possible...");
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
            Console.WriteLine("Generating script to update(recreate all objects) existing MySQLSampleDB from dbAssembly file...");
            dmDataLoss.GeneratePublishScript(s_mysqlDbAssemblyPath, connection, s_mysqlGeneratedPublishRecreateScriptPath);
            Console.WriteLine("Updating(recreate all objects) existing MySQLSampleDB from dbAssembly file...");
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
