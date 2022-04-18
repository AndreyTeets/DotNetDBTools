using System;
using System.Data;
using Dapper;
using DotNetDBTools.Deploy;
using DotNetDBTools.EventsLogger;
using DotNetDBTools.SampleBusinessLogicLib.Agnostic;
using MySqlConnector;
using SqlKata.Compilers;

namespace DotNetDBTools.SampleSelfUpdatingApp.MySQL
{
    public static class Program
    {
        private const string MySQLServerPassword = "Strong(!)Passw0rd";
        private const string MySQLServerHostPort = "5006";
        private const string DatabaseName = "AgnosticSampleDB_SelfUpdatingApp";

        private static readonly string s_connectionString = $"Host=127.0.0.1;Port={MySQLServerHostPort};Database={DatabaseName};Username=root;Password={MySQLServerPassword}";

        public static void Main()
        {
            CreateDatabaseIfNotExists(s_connectionString);
            RegisterDatabaseIfNotRegistered(s_connectionString);

            using MySqlConnection connection = new(s_connectionString);
            PublishAgnosticSampleDBv2(connection);

            MySqlCompiler compiler = new();
            SampleBusinessLogic.ReadWriteSomeData(connection, compiler);
        }

        private static void PublishAgnosticSampleDBv2(IDbConnection connection)
        {
            Console.WriteLine("Publishing DotNetDBTools.SampleDBv2.Agnostic from referenced assembly");
            MySQLDeployManager deployManager = new(new DeployOptions());
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
            MySQLDeployManager deployManager = new(new DeployOptions());
            deployManager.Events.EventFired += DeployManagerEventsLogger.LogEvent;
            using MySqlConnection connection = new(connectionString);

            if (!deployManager.IsRegisteredAsDNDBT(connection))
            {
                Console.WriteLine("Database isn't registered as DNDBT. Registering it.");
                deployManager.RegisterAsDNDBT(connection);
            }
        }

        private static bool DatabaseExists(string connectionString)
        {
            (string databaseName, string connectionStringWithoutDb) = GetDbNameAndConnStringWithoutDb(connectionString);
            using MySqlConnection connection = new(connectionStringWithoutDb);
            return connection.ExecuteScalar<bool>(
$@"SELECT TRUE FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '{databaseName}';");
        }

        private static void CreateDatabase(string connectionString)
        {
            (string databaseName, string connectionStringWithoutDb) = GetDbNameAndConnStringWithoutDb(connectionString);
            using MySqlConnection connection = new(connectionStringWithoutDb);
            connection.Execute(
$@"CREATE DATABASE `{databaseName}`;");
        }

        private static (string, string) GetDbNameAndConnStringWithoutDb(string connectionString)
        {
            MySqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.Database;
            connectionStringBuilder.Database = string.Empty;
            string connectionStringWithoutDb = connectionStringBuilder.ConnectionString;
            return (databaseName, connectionStringWithoutDb);
        }
    }
}
