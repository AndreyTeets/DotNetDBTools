﻿using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using Dapper;
using DotNetDBTools.Deploy;

namespace DotNetDBTools.SampleDeployTool.MSSQL
{
    public class Program
    {
        private const string MsSqlServerPassword = "Strong(!)Passw0rd";
        private const string MsSqlServerHostPort = "5005";
        private const string AgnosticDatabaseName = "AgnosticSampleDB";
        private const string MSSQLDatabaseName = "MSSQLSampleDB";

        private const string RepoRoot = "../../../../../..";
        private static readonly string s_samplesOutputDir = $"{RepoRoot}/SamplesOutput";

        private static readonly string s_agnosticDbAssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDB.Agnostic.dll";
        private static readonly string s_agnosticDbV2AssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDBv2.Agnostic.dll";
        private static readonly string s_agnosticConnectionString = $"Data Source=localhost,{MsSqlServerHostPort};Initial Catalog={AgnosticDatabaseName};Integrated Security=False;User ID=SA;Password={MsSqlServerPassword}";

        private static readonly string s_mssqlDbAssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDB.MSSQL.dll";
        private static readonly string s_mssqlDbV2AssemblyPath = $"{s_samplesOutputDir}/DotNetDBTools.SampleDBv2.MSSQL.dll";
        private static readonly string s_mssqlConnectionString = $"Data Source=localhost,{MsSqlServerHostPort};Initial Catalog={MSSQLDatabaseName};Integrated Security=False;User ID=SA;Password={MsSqlServerPassword}";

        private static readonly string s_agnosticGeneratedPublishToEmptyScriptPath = $"{s_samplesOutputDir}/mssql_generated/AgnosticGeneratedPublishToEmptyScript.sql";
        private static readonly string s_agnosticGeneratedPublishToExistingScriptPath = $"{s_samplesOutputDir}/mssql_generated/AgnosticGeneratedPublishToExistingScript.sql";
        private static readonly string s_agnosticGeneratedPublishFromV1ToV2ScriptPath = $"{s_samplesOutputDir}/mssql_generated/AgnosticGeneratedPublishFromV1ToV2Script.sql";
        private static readonly string s_agnosticGeneratedPublishFromV2ToV1ScriptPath = $"{s_samplesOutputDir}/mssql_generated/AgnosticGeneratedPublishFromV2ToV1Script.sql";
        private static readonly string s_agnosticGeneratedDefinitionFromUnregisteredDir = $"{s_samplesOutputDir}/mssql_generated/AgnosticGeneratedDefinitionFromUnregistered";
        private static readonly string s_agnosticGeneratedDefinitionFromRegisteredDir = $"{s_samplesOutputDir}/mssql_generated/AgnosticGeneratedDefinitionFromRegistered";
        private static readonly string s_agnosticGeneratedPublishRecreateScriptPath = $"{s_samplesOutputDir}/mssql_generated/AgnosticGeneratedPublishRecreateScript.sql";

        private static readonly string s_mssqlGeneratedPublishToEmptyScriptPath = $"{s_samplesOutputDir}/mssql_generated/MSSQLGeneratedPublishToEmptyScript.sql";
        private static readonly string s_mssqlGeneratedPublishToExistingScriptPath = $"{s_samplesOutputDir}/mssql_generated/MSSQLGeneratedPublishToExistingScript.sql";
        private static readonly string s_mssqlGeneratedPublishFromV1ToV2ScriptPath = $"{s_samplesOutputDir}/mssql_generated/MSSQLGeneratedPublishFromV1ToV2Script.sql";
        private static readonly string s_mssqlGeneratedPublishFromV2ToV1ScriptPath = $"{s_samplesOutputDir}/mssql_generated/MSSQLGeneratedPublishFromV2ToV1Script.sql";
        private static readonly string s_mssqlGeneratedDefinitionFromUnregisteredDir = $"{s_samplesOutputDir}/mssql_generated/MSSQLGeneratedDefinitionFromUnregistered";
        private static readonly string s_mssqlGeneratedDefinitionFromRegisteredDir = $"{s_samplesOutputDir}/mssql_generated/MSSQLGeneratedDefinitionFromRegistered";
        private static readonly string s_mssqlGeneratedPublishRecreateScriptPath = $"{s_samplesOutputDir}/mssql_generated/MSSQLGeneratedPublishRecreateScript.sql";

        public static void Main()
        {
            RunAgnosticSampleDBDeployExamples();
            RunMSSQLSampleDBDeployExamples();
        }

        private static void RunAgnosticSampleDBDeployExamples()
        {
            DropDatabaseIfExists(s_agnosticConnectionString);
            IDeployManager deployManager = new MSSQLDeployManager(new DeployOptions { AllowDbCreation = true });
            IDeployManager dmDataLoss = new MSSQLDeployManager(new DeployOptions { AllowDataLoss = true });
            using SqlConnection connection = new(s_agnosticConnectionString);

            Console.WriteLine("Generating script to create new AgnosticSampleDB from dbAssembly file...");
            deployManager.GeneratePublishScript(s_agnosticDbAssemblyPath, s_agnosticConnectionString, s_agnosticGeneratedPublishToEmptyScriptPath);
            Console.WriteLine("Creating new AgnosticSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_agnosticDbAssemblyPath, s_agnosticConnectionString);

            Console.WriteLine("Generating script to update(no changes) existing AgnosticSampleDB from dbAssembly file...");
            deployManager.GeneratePublishScript(s_agnosticDbAssemblyPath, s_agnosticConnectionString, s_agnosticGeneratedPublishToExistingScriptPath);
            Console.WriteLine("Updating(no changes) existing AgnosticSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_agnosticDbAssemblyPath, s_agnosticConnectionString);

            Console.WriteLine("Generating script to update(from v1 to v2) AgnosticSampleDB from the corresponding assembly files...");
            Assembly dbAssembly = Assembly.Load(File.ReadAllBytes(s_agnosticDbAssemblyPath));
            Assembly dbAssemblyV2 = Assembly.Load(File.ReadAllBytes(s_agnosticDbV2AssemblyPath));
            dmDataLoss.GeneratePublishScript(dbAssemblyV2, dbAssembly, s_agnosticGeneratedPublishFromV1ToV2ScriptPath);
            Console.WriteLine("Updating(from v1 to v2) existing AgnosticSampleDB from dbAssembly v2 file...");
            dmDataLoss.PublishDatabase(s_agnosticDbV2AssemblyPath, s_agnosticConnectionString);

            Console.WriteLine("Generating script to update(rollback from v2 to v1) existing AgnosticSampleDB from dbAssembly file...");
            dmDataLoss.GeneratePublishScript(s_agnosticDbAssemblyPath, s_agnosticConnectionString, s_agnosticGeneratedPublishFromV2ToV1ScriptPath);
            Console.WriteLine("Updating(rollback from v2 to v1) AgnosticSampleDB using previously generated script...");
            connection.Execute(File.ReadAllText(s_agnosticGeneratedPublishFromV2ToV1ScriptPath));

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

        private static void RunMSSQLSampleDBDeployExamples()
        {
            DropDatabaseIfExists(s_mssqlConnectionString);
            IDeployManager deployManager = new MSSQLDeployManager(new DeployOptions { AllowDbCreation = true });
            IDeployManager dmDataLoss = new MSSQLDeployManager(new DeployOptions { AllowDataLoss = true });
            using SqlConnection connection = new(s_mssqlConnectionString);

            Console.WriteLine("Generating script to create new MSSQLSampleDB from dbAssembly file...");
            deployManager.GeneratePublishScript(s_mssqlDbAssemblyPath, s_mssqlConnectionString, s_mssqlGeneratedPublishToEmptyScriptPath);
            Console.WriteLine("Creating new MSSQLSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_mssqlDbAssemblyPath, s_mssqlConnectionString);

            Console.WriteLine("Generating script to update(no changes) existing MSSQLSampleDB from dbAssembly file...");
            deployManager.GeneratePublishScript(s_mssqlDbAssemblyPath, s_mssqlConnectionString, s_mssqlGeneratedPublishToExistingScriptPath);
            Console.WriteLine("Updating(no changes) existing MSSQLSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_mssqlDbAssemblyPath, s_mssqlConnectionString);

            Console.WriteLine("Generating script to update(from v1 to v2) MSSQLSampleDB from the corresponding assembly files...");
            Assembly dbAssembly = Assembly.Load(File.ReadAllBytes(s_mssqlDbAssemblyPath));
            Assembly dbAssemblyV2 = Assembly.Load(File.ReadAllBytes(s_mssqlDbV2AssemblyPath));
            dmDataLoss.GeneratePublishScript(dbAssemblyV2, dbAssembly, s_mssqlGeneratedPublishFromV1ToV2ScriptPath);
            Console.WriteLine("Updating(from v1 to v2) existing MSSQLSampleDB from dbAssembly v2 file...");
            dmDataLoss.PublishDatabase(s_mssqlDbV2AssemblyPath, s_mssqlConnectionString);

            Console.WriteLine("Generating script to update(rollback from v2 to v1) existing MSSQLSampleDB from dbAssembly file...");
            dmDataLoss.GeneratePublishScript(s_mssqlDbAssemblyPath, s_mssqlConnectionString, s_mssqlGeneratedPublishFromV2ToV1ScriptPath);
            Console.WriteLine("Updating(rollback from v2 to v1) MSSQLSampleDB using previously generated script...");
            connection.Execute(File.ReadAllText(s_mssqlGeneratedPublishFromV2ToV1ScriptPath));

            Console.WriteLine("Unregistering(=deleting DNDBT system information from DB) MSSQLSampleDB...");
            deployManager.UnregisterAsDNDBT(s_mssqlConnectionString);
            Console.WriteLine("Generating definition from existing unregistered MSSQLSampleDB...");
            deployManager.GenerateDefinition(s_mssqlConnectionString, s_mssqlGeneratedDefinitionFromUnregisteredDir);

            Console.WriteLine("Registiring(=generating and adding new DNDBT system information to DB) MSSQLSampleDB...");
            deployManager.RegisterAsDNDBT(s_mssqlConnectionString);
            Console.WriteLine("Generating definition from existing registered MSSQLSampleDB...");
            deployManager.GenerateDefinition(s_mssqlConnectionString, s_mssqlGeneratedDefinitionFromRegisteredDir);

            Console.WriteLine("Newly created DNDBT system information(by registration) has different IDs for all objects");
            Console.WriteLine("Generating script to update(recreate all objects) existing AgnosticSampleDB from dbAssembly file...");
            dmDataLoss.GeneratePublishScript(s_mssqlDbAssemblyPath, s_mssqlConnectionString, s_mssqlGeneratedPublishRecreateScriptPath);
            Console.WriteLine("Updating(recreate all objects) existing AgnosticSampleDB from dbAssembly file...");
            dmDataLoss.PublishDatabase(s_mssqlDbAssemblyPath, s_mssqlConnectionString);
        }

        private static void DropDatabaseIfExists(string connectionString)
        {
            SqlConnectionStringBuilder sqlConnectionBuilder = new(connectionString);
            string databaseName = sqlConnectionBuilder.InitialCatalog;
            sqlConnectionBuilder.InitialCatalog = string.Empty;
            string connectionStringWithoutDb = sqlConnectionBuilder.ConnectionString;

            using SqlConnection connection = new(connectionStringWithoutDb);
            connection.Execute(
$@"IF EXISTS (SELECT * FROM [sys].[databases] WHERE [name] = '{databaseName}')
BEGIN
    ALTER DATABASE {databaseName}
    SET OFFLINE WITH ROLLBACK IMMEDIATE;

    ALTER DATABASE {databaseName}
    SET ONLINE;

    DROP DATABASE {databaseName};
END;");
        }
    }
}
