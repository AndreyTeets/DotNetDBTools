using System;
using System.Data.Common;
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

        private static readonly string s_connectionString = $"Data Source=localhost,{MsSqlServerHostPort};Initial Catalog={AgnosticDatabaseName};Integrated Security=False;User ID=SA;Password={MsSqlServerPassword}";

        public static void Main()
        {
            CreateAndRegisterDatabaseIfDoesntExist(s_connectionString);

            using SqlConnection connection = new(s_connectionString);
            PublishAgnosticSampleDBv2(connection);

            SqlServerCompiler compiler = new();
            SampleBusinessLogic.ReadWriteSomeData(connection, compiler);
        }

        private static void PublishAgnosticSampleDBv2(DbConnection connection)
        {
            Console.WriteLine("Publishing DotNetDBTools.SampleDBv2.Agnostic from referenced assembly");
            MSSQLDeployManager deployManager = new(new DeployOptions());
            deployManager.PublishDatabase(typeof(SampleDB.Agnostic.Tables.MyTable3).Assembly, connection);
        }

        private static void CreateAndRegisterDatabaseIfDoesntExist(string connectionString)
        {
            if (!DatabaseExists(connectionString))
            {
                Console.WriteLine("Database doesn't exist. Creating new empty database and registering it as DNDBT.");
                CreateDatabase(connectionString);
                using SqlConnection connection = new(connectionString);
                new MSSQLDeployManager(new DeployOptions()).RegisterAsDNDBT(connection);
            }
        }

        private static bool DatabaseExists(string connectionString)
        {
            (string databaseName, string connectionStringWithoutDb) = GetDbNameAndConnStringWithoutDb(connectionString);
            using SqlConnection connection = new(connectionStringWithoutDb);
            return connection.ExecuteScalar<bool>(
$@"SELECT TOP 1 1 FROM [sys].[databases] WHERE [name] = '{databaseName}';");
        }

        private static void CreateDatabase(string connectionString)
        {
            (string databaseName, string connectionStringWithoutDb) = GetDbNameAndConnStringWithoutDb(connectionString);
            using SqlConnection connection = new(connectionStringWithoutDb);
            connection.Execute(
$@"CREATE DATABASE {databaseName};");
        }

        private static (string, string) GetDbNameAndConnStringWithoutDb(string connectionString)
        {
            SqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.InitialCatalog;
            connectionStringBuilder.InitialCatalog = string.Empty;
            string connectionStringWithoutDb = connectionStringBuilder.ConnectionString;
            return (databaseName, connectionStringWithoutDb);
        }
    }
}
