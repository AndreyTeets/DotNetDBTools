using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using DotNetDBTools.Deploy;
using DotNetDBTools.EventsLogger;
using DotNetDBTools.SampleBusinessLogicLib.Agnostic;
using SqlKata.Compilers;

namespace DotNetDBTools.SampleSelfUpdatingApp.MSSQL
{
    public static class Program
    {
        private const string MsSqlServerPassword = "Strong(!)Passw0rd";
        private const string MsSqlServerHostPort = "5005";
        private const string AgnosticDatabaseName = "AgnosticSampleDB_SelfUpdatingApp";

        private static readonly string s_connectionString = $"Data Source=127.0.0.1,{MsSqlServerHostPort};Initial Catalog={AgnosticDatabaseName};Integrated Security=False;User ID=SA;Password={MsSqlServerPassword}";

        public static void Main()
        {
            CreateDatabaseIfNotExists(s_connectionString);
            RegisterDatabaseIfNotRegistered(s_connectionString);

            using SqlConnection connection = new(s_connectionString);
            PublishAgnosticSampleDBv2(connection);

            SqlServerCompiler compiler = new();
            SampleBusinessLogic.ReadWriteSomeData(connection, compiler);
        }

        private static void PublishAgnosticSampleDBv2(IDbConnection connection)
        {
            Console.WriteLine("Publishing DotNetDBTools.SampleDBv2.Agnostic from referenced assembly");
            MSSQLDeployManager deployManager = new(new DeployOptions());
            deployManager.Events.EventFired += DeployManagerEventsLogger.LogEvent;
            deployManager.PublishDatabase(typeof(SampleDB.Agnostic.Tables.MyTable3).Assembly, connection);
        }

        private static void CreateDatabaseIfNotExists(string connectionString)
        {
            if (!DatabaseExists(connectionString))
            {
                Console.WriteLine("Database doesn't exist. Creating new empty database.");
                CreateDatabase(connectionString);
            }
        }

        private static void RegisterDatabaseIfNotRegistered(string connectionString)
        {
            MSSQLDeployManager deployManager = new(new DeployOptions());
            deployManager.Events.EventFired += DeployManagerEventsLogger.LogEvent;
            using SqlConnection connection = new(connectionString);

            if (!deployManager.IsRegisteredAsDNDBT(connection))
            {
                Console.WriteLine("Database isn't registered as DNDBT. Registering it.");
                deployManager.RegisterAsDNDBT(connection);
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
