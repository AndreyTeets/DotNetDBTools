using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DotNetDBTools.TestsUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetDBTools.IntegrationTests.MSSQL
{
    [TestClass]
    public class MsSqlContainerAssemblyFixture
    {
        private const string MsSqlImage = "mcr.microsoft.com/mssql/server";
        private const string MsSqlImageTag = "2019-latest";
        private const string MsSqlContainerName = "DotNetDBTools_IntegrationTests_MSSQL";
        private const string MsSqlServerPassword = "Strong(!)Passw0rd";
        private const string MsSqlServerHostPort = "5005";

        public static string MsSqlContainerConnectionString =>
            new SqlConnectionStringBuilder()
            {
                DataSource = $"localhost,{MsSqlServerHostPort}",
                IntegratedSecurity = false,
                UserID = "SA",
                Password = MsSqlServerPassword,
            }.ConnectionString;

        [AssemblyInitialize]
        public static async Task AssemblyInitialize(TestContext _)
        {
            await DockerHelper.StopAndRemoveContainerIfExistsAndNotRunningOrOld(MsSqlContainerName, oldMinutes: 60);
            await CreateAndStartMsSqlContainerIfNotExists();
            await DbHelper.WaitUntilDatabaseAvailableAsync(new SqlConnection(MsSqlContainerConnectionString), timeoutSeconds: 60);
        }

        private static async Task CreateAndStartMsSqlContainerIfNotExists()
        {
            List<string> envVariables = new()
            {
                "ACCEPT_EULA=Y",
                $"SA_PASSWORD={MsSqlServerPassword}",
            };

            Dictionary<string, string> portRedirects = new()
            {
                { "1433/tcp", MsSqlServerHostPort },
            };

            await DockerHelper.CreateAndStartContainerIfNotExists(
                MsSqlContainerName,
                $"{MsSqlImage}:{MsSqlImageTag}",
                envVariables,
                portRedirects);
        }
    }
}
