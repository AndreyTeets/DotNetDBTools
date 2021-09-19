using System;
using System.Data.SqlClient;
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

        private const string AgnosticGeneratedPublishToEmptyScriptPath = "./generated/AgnosticGeneratedPublishToEmptyScript.sql";
        private const string AgnosticGeneratedPublishToExistingScriptPath = "./generated/AgnosticGeneratedPublishToExistingScript.sql";
        private const string AgnosticGeneratedDefinitionFromUnregisteredDirectory = "./generated/AgnosticGeneratedDefinitionFromUnregisteredDirectory";
        private const string AgnosticGeneratedDefinitionFromRegisteredDirectory = "./generated/AgnosticGeneratedDefinitionFromRegisteredDirectory";
        private const string MSSQLGeneratedPublishToEmptyScriptPath = "./generated/MSSQLGeneratedPublishToEmptyScript.sql";
        private const string MSSQLGeneratedPublishToExistingScriptPath = "./generated/MSSQLGeneratedPublishToExistingScript.sql";
        private const string MSSQLGeneratedDefinitionFromUnregisteredDirectory = "./generated/MSSQLGeneratedDefinitionFromUnregisteredDirectory";
        private const string MSSQLGeneratedDefinitionFromRegisteredDirectory = "./generated/MSSQLGeneratedDefinitionFromRegisteredDirectory";
        private const string RepoRoot = "../../../../..";

        private static readonly string s_agnosticDbProjectBinDir = $"{RepoRoot}/samples/DotNetDBTools.SampleDB.Agnostic/bin";
        private static readonly string s_agnosticDbAssemblyPath = $"{s_agnosticDbProjectBinDir}/DbAssembly/DotNetDBTools.SampleDB.Agnostic.dll";
        private static readonly string s_agnosticConnectionString = $"Data Source=localhost,{MsSqlServerHostPort};Initial Catalog={AgnosticDatabaseName};Integrated Security=False;User ID=SA;Password={MsSqlServerPassword}";

        private static readonly string s_mssqlDbProjectBinDir = $"{RepoRoot}/samples/DotNetDBTools.SampleDB.MSSQL/bin";
        private static readonly string s_mssqlDbAssemblyPath = $"{s_mssqlDbProjectBinDir}/DbAssembly/DotNetDBTools.SampleDB.MSSQL.dll";
        private static readonly string s_mssqlConnectionString = $"Data Source=localhost,{MsSqlServerHostPort};Initial Catalog={MSSQLDatabaseName};Integrated Security=False;User ID=SA;Password={MsSqlServerPassword}";

        public static void Main()
        {
            RunAgnosticSampleDBDeployExamples();
            RunMSSQLSampleDBDeployExamples();
        }

        private static void RunAgnosticSampleDBDeployExamples()
        {
            DropDatabaseIfExists(s_agnosticConnectionString);
            IDeployManager deployManager = new MSSQLDeployManager(true, false);

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

        private static void RunMSSQLSampleDBDeployExamples()
        {
            DropDatabaseIfExists(s_mssqlConnectionString);
            IDeployManager deployManager = new MSSQLDeployManager(true, false);

            Console.WriteLine("Generating create new MSSQLSampleDB from dbAssembly file...");
            deployManager.GeneratePublishScript(s_mssqlDbAssemblyPath, s_mssqlConnectionString, MSSQLGeneratedPublishToEmptyScriptPath);
            Console.WriteLine("Creating new MSSQLSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_mssqlDbAssemblyPath, s_mssqlConnectionString);

            Console.WriteLine("Generating update(no changes) existing MSSQLSampleDB from dbAssembly file...");
            deployManager.GeneratePublishScript(s_mssqlDbAssemblyPath, s_mssqlConnectionString, MSSQLGeneratedPublishToExistingScriptPath);
            Console.WriteLine("Updating(no changes) existing MSSQLSampleDB from dbAssembly file...");
            deployManager.PublishDatabase(s_mssqlDbAssemblyPath, s_mssqlConnectionString);

            Console.WriteLine("Unregistering(=deleting DNDBT system information from DB) MSSQLSampleDB...");
            deployManager.UnregisterAsDNDBT(s_mssqlConnectionString);
            Console.WriteLine("Generating definition from existing unregistered MSSQLSampleDB...");
            deployManager.GenerateDefinition(s_mssqlConnectionString, MSSQLGeneratedDefinitionFromUnregisteredDirectory);

            Console.WriteLine("Registiring(=generating and adding new DNDBT system information to DB) MSSQLSampleDB...");
            deployManager.RegisterAsDNDBT(s_mssqlConnectionString);
            Console.WriteLine("Generating definition from existing registered MSSQLSampleDB...");
            deployManager.GenerateDefinition(s_mssqlConnectionString, MSSQLGeneratedDefinitionFromRegisteredDirectory);
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
