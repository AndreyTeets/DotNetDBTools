using System;
using System.Data.SqlClient;
using Dapper;
using DotNetDBTools.Deploy;
using DotNetDBTools.SampleBusinessLogicLib.Agnostic;
using SqlKata.Compilers;

namespace DotNetDBTools.SampleSelfUpdatingApp.MSSQL
{
    public static class Program
    {
        private const string MsSqlServerPassword = "Strong(!)Passw0rd";
        private const string MsSqlServerHostPort = "5005";
        private const string AgnosticDatabaseName = "AgnosticSampleDB_SelfUpdatingApp";

        private static readonly string s_agnosticConnectionString = $"Data Source=localhost,{MsSqlServerHostPort};Initial Catalog={AgnosticDatabaseName};Integrated Security=False;User ID=SA;Password={MsSqlServerPassword}";

        public static void Main()
        {
            DropDatabaseIfExists(s_agnosticConnectionString);
            CreateDatabase(s_agnosticConnectionString);

            Console.WriteLine("Creating new AgnosticSampleDB_SelfUpdatingApp v2 from dbAssembly file...");
            SqlConnection connection = new(s_agnosticConnectionString);
            DeployAgnosticSampleDB(connection);

            SqlServerCompiler compiler = new();
            SampleBusinessLogic.ReadWriteSomeData(connection, compiler);
        }

        private static void DeployAgnosticSampleDB(SqlConnection connection)
        {
            MSSQLDeployManager deployManager = new(new DeployOptions());
            deployManager.RegisterAsDNDBT(connection);
            deployManager.PublishDatabase(typeof(SampleDB.Agnostic.Tables.MyTable3).Assembly, connection);
        }

        private static void CreateDatabase(string connectionString)
        {
            SqlConnectionStringBuilder sqlConnectionBuilder = new(connectionString);
            string databaseName = sqlConnectionBuilder.InitialCatalog;
            sqlConnectionBuilder.InitialCatalog = string.Empty;
            string connectionStringWithoutDb = sqlConnectionBuilder.ConnectionString;

            using SqlConnection connection = new(connectionStringWithoutDb);
            connection.Execute(
$@"CREATE DATABASE {databaseName};");
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
