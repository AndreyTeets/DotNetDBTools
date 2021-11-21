using System;
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

        private static readonly string s_agnosticConnectionString = $"Host=localhost;Port={MySQLServerHostPort};Database={DatabaseName};Username=root;Password={MySQLServerPassword}";

        public static void Main()
        {
            DropDatabaseIfExists(s_agnosticConnectionString);
            CreateDatabase(s_agnosticConnectionString);

            Console.WriteLine("Creating new AgnosticSampleDB_SelfUpdatingApp v2 from dbAssembly file...");
            using MySqlConnection connection = new(s_agnosticConnectionString);
            DeployAgnosticSampleDB(connection);

            MySqlCompiler compiler = new();
            SampleBusinessLogic.ReadWriteSomeData(connection, compiler);
        }

        private static void DeployAgnosticSampleDB(MySqlConnection connection)
        {
            MySQLDeployManager deployManager = new(new DeployOptions());
            deployManager.RegisterAsDNDBT(connection);
            deployManager.PublishDatabase(typeof(SampleDB.Agnostic.Tables.MyTable3).Assembly, connection);
        }

        private static void CreateDatabase(string connectionString)
        {
            MySqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.Database;
            connectionStringBuilder.Database = string.Empty;
            string connectionStringWithoutDb = connectionStringBuilder.ConnectionString;

            using MySqlConnection connection = new(connectionStringWithoutDb);
            connection.Execute(
$@"CREATE DATABASE `{databaseName}`;");
        }

        private static void DropDatabaseIfExists(string connectionString)
        {
            MySqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.Database;
            connectionStringBuilder.Database = string.Empty;
            string connectionStringWithoutDb = connectionStringBuilder.ConnectionString;

            using MySqlConnection connection = new(connectionStringWithoutDb);
            connection.Execute(
$@"DROP DATABASE IF EXISTS `{databaseName}`;");
        }
    }
}
