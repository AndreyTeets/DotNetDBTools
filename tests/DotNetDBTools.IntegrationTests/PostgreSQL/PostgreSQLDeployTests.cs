using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql;

namespace DotNetDBTools.IntegrationTests.PostgreSQL
{
    [TestClass]
    public class PostgreSQLDeployTests
    {
        private static readonly string s_connectionStringWithoutDb = PostgreSQLContainerHelper.PostgreSQLContainerConnectionString;

        private string ConnectionString => CreateConnectionString(s_connectionStringWithoutDb, TestContext.TestName);
        public TestContext TestContext { get; set; }

        private NpgsqlConnection _connection;

        [TestInitialize]
        public void TestInitialize()
        {
            DropDatabaseIfExists(ConnectionString);
            CreateDatabase(ConnectionString);

            _connection = new(ConnectionString);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _connection.Dispose();
        }

        [TestMethod]
        public void DbSetupWorks()
        {
            Assert.IsTrue(true);
        }

        private static void CreateDatabase(string connectionString)
        {
            NpgsqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.Database;

            using NpgsqlConnection connection = new(s_connectionStringWithoutDb);
            connection.Execute(
$@"CREATE DATABASE ""{databaseName}"";");
        }

        private static void DropDatabaseIfExists(string connectionString)
        {
            NpgsqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.Database;

            using NpgsqlConnection connection = new(s_connectionStringWithoutDb);
            connection.Execute(
$@"SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = '{databaseName}';
DROP DATABASE IF EXISTS ""{databaseName}"";");
        }

        private static string CreateConnectionString(string connectionStringWithoutDb, string databaseName)
        {
            NpgsqlConnectionStringBuilder connectionStringBuilder = new(connectionStringWithoutDb);
            connectionStringBuilder.Database = databaseName;
            string connectionString = connectionStringBuilder.ConnectionString;
            return connectionString;
        }
    }
}
