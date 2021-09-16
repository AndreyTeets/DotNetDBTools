using System;
using System.Data.SqlClient;
using Dapper;
using DotNetDBTools.Deploy.MSSQL;

namespace DotNetDBTools.SampleDeployUtil.MSSQL
{
    public class Program
    {
        private const string MsSqlServerPassword = "Strong(!)Passw0rd";
        private const string MsSqlServerHostPort = "5005";
        private const string AgnosticDatabaseName = "AgnosticSampleDB";
        private const string MSSQLDatabaseName = "MSSQLSampleDB";

        private const string RepoRoot = "../../../../../..";

        private static readonly string s_agnosticDbProjectBinDir = $"{RepoRoot}/samples/Databases/DotNetDBTools.SampleDB.Agnostic/bin";
        private static readonly string s_agnosticDbAssemblyPath = $"{s_agnosticDbProjectBinDir}/DbAssembly/DotNetDBTools.SampleDB.Agnostic.dll";
        private static readonly string s_agnosticConnectionString = $"Data Source=localhost,{MsSqlServerHostPort};Initial Catalog={AgnosticDatabaseName};Integrated Security=False;User ID=SA;Password={MsSqlServerPassword}";

        private static readonly string s_mssqlDbProjectBinDir = $"{RepoRoot}/samples/Databases/DotNetDBTools.SampleDB.MSSQL/bin";
        private static readonly string s_mssqlDbAssemblyPath = $"{s_mssqlDbProjectBinDir}/DbAssembly/DotNetDBTools.SampleDB.MSSQL.dll";
        private static readonly string s_mssqlConnectionString = $"Data Source=localhost,{MsSqlServerHostPort};Initial Catalog={MSSQLDatabaseName};Integrated Security=False;User ID=SA;Password={MsSqlServerPassword}";

        public static void Main()
        {
            RunAgnosticSampleDBExample();
            RunMSSQLSampleDBExample();
        }

        private static void RunAgnosticSampleDBExample()
        {
            DropDatabaseIfExists(s_agnosticConnectionString);

            Console.WriteLine("Creating new AgnosticSampleDB...");
            DeployAgnosticSampleDB();

            Console.WriteLine("Updating existing AgnosticSampleDB...");
            DeployAgnosticSampleDB();
        }

        private static void RunMSSQLSampleDBExample()
        {
            DropDatabaseIfExists(s_mssqlConnectionString);

            Console.WriteLine("Creating new MSSQLSampleDB...");
            DeployMSSQLSampleDB();

            Console.WriteLine("Updating existing MSSQLSampleDB...");
            DeployMSSQLSampleDB();
        }

        private static void DeployAgnosticSampleDB()
        {
            MSSQLDeployManager deployManager = new(true, false);
            deployManager.UpdateDatabase(s_agnosticDbAssemblyPath, s_agnosticConnectionString);
        }

        private static void DeployMSSQLSampleDB()
        {
            MSSQLDeployManager deployManager = new(true, false);
            deployManager.UpdateDatabase(s_mssqlDbAssemblyPath, s_mssqlConnectionString);
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
