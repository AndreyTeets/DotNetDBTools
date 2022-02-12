using System;
using System.Data.Common;
using Dapper;
using DotNetDBTools.Deploy;
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

        private static readonly string s_connectionString = $"Host=localhost;Port={MySQLServerHostPort};Database={DatabaseName};Username=root;Password={MySQLServerPassword}";

        public static void Main()
        {
            CreateAndRegisterDatabaseIfDoesntExist(s_connectionString);

            using MySqlConnection connection = new(s_connectionString);
            PublishAgnosticSampleDBv2(connection);

            MySqlCompiler compiler = new();
            SampleBusinessLogic.ReadWriteSomeData(connection, compiler);
        }

        private static void PublishAgnosticSampleDBv2(DbConnection connection)
        {
            Console.WriteLine("Publishing DotNetDBTools.SampleDBv2.Agnostic from referenced assembly");
            MySQLDeployManager deployManager = new(new DeployOptions());
            deployManager.PublishDatabase(typeof(SampleDB.Agnostic.Tables.MyTable3).Assembly, connection);
        }

        private static void CreateAndRegisterDatabaseIfDoesntExist(string connectionString)
        {
            if (!DatabaseExists(connectionString))
            {
                Console.WriteLine("Database doesn't exist. Creating new empty database and registering it as DNDBT.");
                CreateDatabase(connectionString);
                using MySqlConnection connection = new(connectionString);
                new MySQLDeployManager(new DeployOptions()).RegisterAsDNDBT(connection);
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
