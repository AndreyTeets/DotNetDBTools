using System;
using System.Data.Common;
using Dapper;
using DotNetDBTools.Deploy;
using DotNetDBTools.SampleBusinessLogicLib.Agnostic;
using Npgsql;
using SqlKata.Compilers;

namespace DotNetDBTools.SampleSelfUpdatingApp.PostgreSQL
{
    public static class Program
    {
        private const string PostgreSQLServerPassword = "Strong(!)Passw0rd";
        private const string PostgreSQLServerHostPort = "5007";
        private const string DatabaseName = "AgnosticSampleDB_SelfUpdatingApp";

        private static readonly string s_connectionString = $"Host=localhost;Port={PostgreSQLServerHostPort};Database={DatabaseName};Username=postgres;Password={PostgreSQLServerPassword}";

        public static void Main()
        {
            CreateAndRegisterDatabaseIfDoesntExist(s_connectionString);

            using NpgsqlConnection connection = new(s_connectionString);
            PublishAgnosticSampleDBv2(connection);

            PostgresCompiler compiler = new();
            SampleBusinessLogic.ReadWriteSomeData(connection, compiler);
        }

        private static void PublishAgnosticSampleDBv2(DbConnection connection)
        {
            Console.WriteLine("Publishing DotNetDBTools.SampleDBv2.Agnostic from referenced assembly");
            PostgreSQLDeployManager deployManager = new(new DeployOptions());
            deployManager.PublishDatabase(typeof(SampleDB.Agnostic.Tables.MyTable3).Assembly, connection);
        }

        private static void CreateAndRegisterDatabaseIfDoesntExist(string connectionString)
        {
            if (!DatabaseExists(connectionString))
            {
                Console.WriteLine("Database doesn't exist. Creating new empty database and registering it as DNDBT.");
                CreateDatabase(connectionString);
                using NpgsqlConnection connection = new(connectionString);
                new PostgreSQLDeployManager(new DeployOptions()).RegisterAsDNDBT(connection);
            }
        }

        private static bool DatabaseExists(string connectionString)
        {
            (string databaseName, string connectionStringWithoutDb) = GetDbNameAndConnStringWithoutDb(connectionString);
            using NpgsqlConnection connection = new(connectionStringWithoutDb);
            return connection.ExecuteScalar<bool>(
$@"SELECT TRUE FROM pg_catalog.pg_database WHERE datname = '{databaseName}';");
        }

        private static void CreateDatabase(string connectionString)
        {
            (string databaseName, string connectionStringWithoutDb) = GetDbNameAndConnStringWithoutDb(connectionString);
            using NpgsqlConnection connection = new(connectionStringWithoutDb);
            connection.Execute(
$@"CREATE DATABASE ""{databaseName}"";");
        }

        private static (string, string) GetDbNameAndConnStringWithoutDb(string connectionString)
        {
            NpgsqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.Database;
            connectionStringBuilder.Database = string.Empty;
            string connectionStringWithoutDb = connectionStringBuilder.ConnectionString;
            return (databaseName, connectionStringWithoutDb);
        }
    }
}
