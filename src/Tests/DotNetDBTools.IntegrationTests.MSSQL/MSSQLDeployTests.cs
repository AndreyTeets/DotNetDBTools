using System.Data.SqlClient;
using Dapper;
using DotNetDBTools.Deploy.MSSQL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetDBTools.IntegrationTests.MSSQL
{
    [TestClass]
    public class MSSQLDeployTests
    {
        private const string AgnosticSampleDbAssemblyPath = "../../../../../Samples/DotNetDBTools.SampleDB.Agnostic/bin/DbAssembly/DotNetDBTools.SampleDB.Agnostic.dll";
        private const string MSSQLSampleDbAssemblyPath = "../../../../../Samples/DotNetDBTools.SampleDB.MSSQL/bin/DbAssembly/DotNetDBTools.SampleDB.MSSQL.dll";
        private static readonly string s_connectionStringWithoutDb = MsSqlContainerAssemblyFixture.MsSqlContainerConnectionString;

        private string ConnectionString => CreateConnectionString(s_connectionStringWithoutDb, TestContext.TestName);
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Update_AgnosticSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            DropDatabaseIfExists(ConnectionString);
            MSSQLDeployManager deployManager = new(true, false);
            deployManager.UpdateDatabase(AgnosticSampleDbAssemblyPath, ConnectionString);
            deployManager.UpdateDatabase(AgnosticSampleDbAssemblyPath, ConnectionString);
        }

        [TestMethod]
        public void Update_MSSQLSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            DropDatabaseIfExists(ConnectionString);
            MSSQLDeployManager deployManager = new(true, false);
            deployManager.UpdateDatabase(MSSQLSampleDbAssemblyPath, ConnectionString);
            deployManager.UpdateDatabase(MSSQLSampleDbAssemblyPath, ConnectionString);
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
