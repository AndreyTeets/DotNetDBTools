using System.Data.SqlClient;
using Dapper;
using DotNetDBTools.Deploy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.MSSQL
{
    [TestClass]
    public class MSSQLDeployTests
    {
        private static readonly string s_connectionStringWithoutDb = MSSQLContainerAssemblyFixture.MsSqlContainerConnectionString;

        private static readonly string s_agnosticSampleDbAssemblyPath = $"{SamplesOutputDir}/DotNetDBTools.SampleDB.Agnostic.dll";
        private static readonly string s_mssqlSampleDbAssemblyPath = $"{SamplesOutputDir}/DotNetDBTools.SampleDB.MSSQL.dll";

        private string ConnectionString => CreateConnectionString(s_connectionStringWithoutDb, TestContext.TestName);
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Publish_AgnosticSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            DropDatabaseIfExists(ConnectionString);
            MSSQLDeployManager deployManager = new(true, false);
            deployManager.PublishDatabase(s_agnosticSampleDbAssemblyPath, ConnectionString);
            deployManager.PublishDatabase(s_agnosticSampleDbAssemblyPath, ConnectionString);
        }

        [TestMethod]
        public void Publish_MSSQLSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            DropDatabaseIfExists(ConnectionString);
            MSSQLDeployManager deployManager = new(true, false);
            deployManager.PublishDatabase(s_mssqlSampleDbAssemblyPath, ConnectionString);
            deployManager.PublishDatabase(s_mssqlSampleDbAssemblyPath, ConnectionString);
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

        private static string CreateConnectionString(string connectionStringWithoutDb, string databaseName)
        {
            SqlConnectionStringBuilder sqlConnectionBuilder = new(connectionStringWithoutDb);
            sqlConnectionBuilder.InitialCatalog = databaseName;
            string connectionString = sqlConnectionBuilder.ConnectionString;
            return connectionString;
        }
    }
}
