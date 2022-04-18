using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using Dapper;
using DotNetDBTools.Deploy;
using DotNetDBTools.EventsLogger;

namespace DotNetDBTools.SampleDeployManagerUsage.MSSQL
{
    public static class Program
    {
        private const string MsSqlServerPassword = "Strong(!)Passw0rd";
        private const string MsSqlServerHostPort = "5005";
        private const string MSSQLDatabaseName = "MSSQLSampleDB_BusinessLogicOnlyApp";

        private const string RepoRoot = "../../../../../..";
        private static readonly string s_samplesOutputDir = $"{RepoRoot}/SamplesOutput";
        private static readonly string s_generatedOutputDir = $"{s_samplesOutputDir}/mssql_generated";

        private static readonly string s_mssqlDbAssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDB.MSSQL.dll";
        private static readonly string s_mssqlDbV2AssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDBv2.MSSQL.dll";
        private static readonly string s_mssqlConnectionString = $"Data Source=127.0.0.1,{MsSqlServerHostPort};Initial Catalog={MSSQLDatabaseName};Integrated Security=False;User ID=SA;Password={MsSqlServerPassword}";

        private static readonly string s_publishToEmptyScriptPath = $"{s_generatedOutputDir}/PublishToEmptyScript.sql";
        private static readonly string s_publishToExistingScriptPath = $"{s_generatedOutputDir}/PublishToExistingScript.sql";
        private static readonly string s_publishFromV1ToV2ScriptPath = $"{s_generatedOutputDir}/PublishFromV1ToV2Script.sql";
        private static readonly string s_noDNDBTInfoPublishFromV1ToV2ScriptPath = $"{s_generatedOutputDir}/NoDNDBTInfoPublishFromV1ToV2Script.sql";
        private static readonly string s_publishFromV2ToV1ScriptPath = $"{s_generatedOutputDir}/PublishFromV2ToV1Script.sql";
        private static readonly string s_noDNDBTInfoPublishFromV2ToV1ScriptPath = $"{s_generatedOutputDir}/NoDNDBTInfoPublishFromV2ToV1Script.sql";
        private static readonly string s_definitionFromUnregisteredDir = $"{s_generatedOutputDir}/DefinitionFromUnregistered";
        private static readonly string s_definitionFromRegisteredDir = $"{s_generatedOutputDir}/DefinitionFromRegistered";
        private static readonly string s_publishRecreateScriptPath = $"{s_generatedOutputDir}/PublishRecreateScript.sql";

        public static void Main()
        {
            RunMSSQLSampleDBDeployExamples();
        }

        private static void RunMSSQLSampleDBDeployExamples()
        {
            DropDatabaseIfExists(s_mssqlConnectionString);
            CreateDatabase(s_mssqlConnectionString);
            Directory.CreateDirectory(s_generatedOutputDir);

            Assembly dbAssembly = Assembly.Load(File.ReadAllBytes(s_mssqlDbAssemblyPath));
            Assembly dbAssemblyV2 = Assembly.Load(File.ReadAllBytes(s_mssqlDbV2AssemblyPath));

            IDeployManager deployManager = new MSSQLDeployManager(new DeployOptions());
            deployManager.Events.EventFired += DeployManagerEventsLogger.LogEvent;
            IDeployManager dmDataLoss = new MSSQLDeployManager(new DeployOptions { AllowDataLoss = true });
            dmDataLoss.Events.EventFired += DeployManagerEventsLogger.LogEvent;
            using SqlConnection connection = new(s_mssqlConnectionString);

            Console.WriteLine("Registering empty MSSQLSampleDB as DNDBT to make other actions with it possible...");
            deployManager.RegisterAsDNDBT(connection);

            Console.WriteLine("Generating script to create new MSSQLSampleDB from dbAssembly file...");
            File.WriteAllText(s_publishToEmptyScriptPath, deployManager.GeneratePublishScript(s_mssqlDbAssemblyPath, connection));
            Console.WriteLine("Creating new MSSQLSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_mssqlDbAssemblyPath, connection);

            Console.WriteLine("Generating script to update(no changes) existing MSSQLSampleDB from dbAssembly file...");
            File.WriteAllText(s_publishToExistingScriptPath, deployManager.GeneratePublishScript(s_mssqlDbAssemblyPath, connection));
            Console.WriteLine("Updating(no changes) existing MSSQLSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_mssqlDbAssemblyPath, connection);

            Console.WriteLine("Generating script to update(from v1 to v2) MSSQLSampleDB from the corresponding assembly files...");
            File.WriteAllText(s_publishFromV1ToV2ScriptPath, dmDataLoss.GeneratePublishScript(dbAssemblyV2, dbAssembly));
            Console.WriteLine("Updating(from v1 to v2) existing MSSQLSampleDB from dbAssembly v2 file...");
            dmDataLoss.PublishDatabase(s_mssqlDbV2AssemblyPath, connection);

            Console.WriteLine("Generating script to update(rollback from v2 to v1) existing MSSQLSampleDB from dbAssembly file...");
            File.WriteAllText(s_publishFromV2ToV1ScriptPath, dmDataLoss.GeneratePublishScript(s_mssqlDbAssemblyPath, connection));
            Console.WriteLine("Updating(rollback from v2 to v1) MSSQLSampleDB using previously generated script...");
            connection.Execute(File.ReadAllText(s_publishFromV2ToV1ScriptPath));

            Console.WriteLine("Unregistering(=deleting DNDBT system information from DB) MSSQLSampleDB...");
            deployManager.UnregisterAsDNDBT(connection);
            Console.WriteLine("Generating definition from existing unregistered MSSQLSampleDB...");
            deployManager.GenerateDefinition(connection, s_definitionFromUnregisteredDir);

            Console.WriteLine("Generating NoDNDBTInfo script to update(from v1 to v2) MSSQLSampleDB from the corresponding assembly files...");
            File.WriteAllText(s_noDNDBTInfoPublishFromV1ToV2ScriptPath, dmDataLoss.GenerateNoDNDBTInfoPublishScript(dbAssemblyV2, dbAssembly));
            Console.WriteLine("Updating(from v1 to v2) MSSQLSampleDB using previously generated NoDNDBTInfo script...");
            connection.Execute(File.ReadAllText(s_noDNDBTInfoPublishFromV1ToV2ScriptPath));

            Console.WriteLine("Generating NoDNDBTInfo script to update(rollback from v2 to v1) MSSQLSampleDB from the corresponding assembly files...");
            File.WriteAllText(s_noDNDBTInfoPublishFromV2ToV1ScriptPath, dmDataLoss.GenerateNoDNDBTInfoPublishScript(dbAssembly, dbAssemblyV2));
            Console.WriteLine("Updating(rollback from v2 to v1) MSSQLSampleDB using previously generated NoDNDBTInfo script...");
            connection.Execute(File.ReadAllText(s_noDNDBTInfoPublishFromV2ToV1ScriptPath));

            Console.WriteLine("Registering(=generating and adding new DNDBT system information to DB) MSSQLSampleDB...");
            deployManager.RegisterAsDNDBT(connection);
            Console.WriteLine("Generating definition from existing registered MSSQLSampleDB...");
            deployManager.GenerateDefinition(connection, s_definitionFromRegisteredDir);

            Console.WriteLine("Newly created DNDBT system information(by registration) has different IDs for all objects");
            Console.WriteLine("Generating script to update(recreate all objects) existing MSSQLSampleDB from dbAssembly file...");
            File.WriteAllText(s_publishRecreateScriptPath, dmDataLoss.GeneratePublishScript(s_mssqlDbAssemblyPath, connection));
            Console.WriteLine("Updating(recreate all objects) existing MSSQLSampleDB from dbAssembly file...");
            dmDataLoss.PublishDatabase(s_mssqlDbAssemblyPath, connection);
        }

        private static void CreateDatabase(string connectionString)
        {
            SqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.InitialCatalog;
            connectionStringBuilder.InitialCatalog = string.Empty;
            string connectionStringWithoutDb = connectionStringBuilder.ConnectionString;

            using SqlConnection connection = new(connectionStringWithoutDb);
            connection.Execute(
$@"CREATE DATABASE {databaseName};");
        }

        private static void DropDatabaseIfExists(string connectionString)
        {
            SqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.InitialCatalog;
            connectionStringBuilder.InitialCatalog = string.Empty;
            string connectionStringWithoutDb = connectionStringBuilder.ConnectionString;

            using SqlConnection connection = new(connectionStringWithoutDb);
            connection.Execute(
$@"IF EXISTS (SELECT * FROM [sys].[databases] WHERE [name] = '{databaseName}')
BEGIN
    ALTER DATABASE {databaseName}
    SET SINGLE_USER WITH ROLLBACK IMMEDIATE;

    DROP DATABASE {databaseName};
END;");
        }
    }
}
