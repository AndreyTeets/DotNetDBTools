using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySqlConnector;

namespace DotNetDBTools.IntegrationTests.MySQL
{
    [TestClass]
    public class MySQLDeployTests
    {
        private static readonly string s_connectionStringWithoutDb = MySQLContainerHelper.MySQLContainerConnectionString;

        private string ConnectionString => CreateConnectionString(s_connectionStringWithoutDb, TestContext.TestName);
        public TestContext TestContext { get; set; }

        private MySqlConnection _connection;

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
            MySqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.Database;

            using MySqlConnection connection = new(s_connectionStringWithoutDb);
            connection.Execute(
$@"CREATE DATABASE `{databaseName}`;");
        }

        private static void DropDatabaseIfExists(string connectionString)
        {
            MySqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.Database;

            using MySqlConnection connection = new(s_connectionStringWithoutDb);
            connection.Execute(
$@"DROP DATABASE IF EXISTS `{databaseName}`;");
        }

        private static string CreateConnectionString(string connectionStringWithoutDb, string databaseName)
        {
            MySqlConnectionStringBuilder connectionStringBuilder = new(connectionStringWithoutDb);
            connectionStringBuilder.Database = databaseName;
            string connectionString = connectionStringBuilder.ConnectionString;
            return connectionString;
        }
    }
}
