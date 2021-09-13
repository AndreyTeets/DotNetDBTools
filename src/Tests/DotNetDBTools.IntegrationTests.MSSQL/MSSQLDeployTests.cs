using System.Data.SqlClient;
using System.Reflection;
using Dapper;
using DotNetDBTools.Deploy.MSSQL;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetDBTools.IntegrationTests.MSSQL
{
    [TestClass]
    public class MSSQLDeployTests
    {
#if DEBUG
        private const string Configuration = "Debug";
#else
        private const string Configuration = "Release";
#endif
        private const string DeployAssemblyBinDir = "../../../../../Samples/DotNetDBTools.SampleDeployUtil.MSSQL/bin";
        private static readonly string s_deployAssemblyPath = $"{DeployAssemblyBinDir}/{Configuration}/netcoreapp3.1/DotNetDBTools.SampleDeployUtil.MSSQL.exe";
        private static readonly string s_connectionStringWithoutDb = MsSqlContainerAssemblyFixture.MsSqlContainerConnectionString;

        private string ConnectionString => CreateConnectionString(s_connectionStringWithoutDb, TestContext.TestName);
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Sample_Projects_Run_OnNew_And_OnExisting_Databases_WithoutErrors()
        {
            ProcessRunner processRunner = new();

            (int exitCodeDeploy, string outputDeploy) = processRunner.RunProcess(s_deployAssemblyPath);
            exitCodeDeploy.Should().Be(0, $"process output: '{outputDeploy}'");
        }

        [TestMethod]
        public void Update_AgnosticSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            DropDatabaseIfExists(ConnectionString);
            Assembly dbAssembly = Assembly.GetAssembly(typeof(SampleDB.Agnostic.Tables.MyTable1));
            MSSQLDeployManager deployManager = new(true, false);
            deployManager.UpdateDatabase(dbAssembly, ConnectionString);
            deployManager.UpdateDatabase(dbAssembly, ConnectionString);
        }

        [TestMethod]
        public void Update_MSSQLSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            DropDatabaseIfExists(ConnectionString);
            Assembly dbAssembly = Assembly.GetAssembly(typeof(SampleDB.MSSQL.Tables.MyTable1));
            MSSQLDeployManager deployManager = new(true, false);
            deployManager.UpdateDatabase(dbAssembly, ConnectionString);
            deployManager.UpdateDatabase(dbAssembly, ConnectionString);
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
