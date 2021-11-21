using System;
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

        private static readonly string s_agnosticConnectionString = $"Host=localhost;Port={PostgreSQLServerHostPort};Database={DatabaseName};Username=postgres;Password={PostgreSQLServerPassword}";

        public static void Main()
        {
            DropDatabaseIfExists(s_agnosticConnectionString);
            CreateDatabase(s_agnosticConnectionString);

            Console.WriteLine("Creating new AgnosticSampleDB_SelfUpdatingApp v2 from dbAssembly file...");
            using NpgsqlConnection connection = new(s_agnosticConnectionString);
            DeployAgnosticSampleDB(connection);

            PostgresCompiler compiler = new();
            SampleBusinessLogic.ReadWriteSomeData(connection, compiler);
        }

        private static void DeployAgnosticSampleDB(NpgsqlConnection connection)
        {
            PostgreSQLDeployManager deployManager = new(new DeployOptions());
            deployManager.RegisterAsDNDBT(connection);
            deployManager.PublishDatabase(typeof(SampleDB.Agnostic.Tables.MyTable3).Assembly, connection);
        }

        private static void CreateDatabase(string connectionString)
        {
            NpgsqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.Database;
            connectionStringBuilder.Database = string.Empty;
            string connectionStringWithoutDb = connectionStringBuilder.ConnectionString;

            using NpgsqlConnection connection = new(connectionStringWithoutDb);
            connection.Execute(
$@"CREATE DATABASE ""{databaseName}"";");
        }

        private static void DropDatabaseIfExists(string connectionString)
        {
            NpgsqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.Database;
            connectionStringBuilder.Database = string.Empty;
            string connectionStringWithoutDb = connectionStringBuilder.ConnectionString;

            using NpgsqlConnection connection = new(connectionStringWithoutDb);
            connection.Execute(
$@"SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = '{databaseName}';
DROP DATABASE IF EXISTS ""{databaseName}"";");
        }
    }
}
