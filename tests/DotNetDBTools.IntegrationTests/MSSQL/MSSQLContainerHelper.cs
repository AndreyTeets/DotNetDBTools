using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DotNetDBTools.IntegrationTests.TestHelpers;

namespace DotNetDBTools.IntegrationTests.MSSQL
{
    public class MSSQLContainerHelper
    {
        private const string MsSqlImage = "mcr.microsoft.com/mssql/server";
        private const string MsSqlImageTag = "2019-CU13-ubuntu-20.04";
        private const string MsSqlContainerName = "DotNetDBTools_IntegrationTests_MSSQL";
        private const string MsSqlServerPassword = "Strong(!)Passw0rd";
        private const int MsSqlServerHostPort = 5005;

        public static string MsSqlContainerConnectionString =>
            new SqlConnectionStringBuilder()
            {
                DataSource = $"localhost,{MsSqlServerHostPort}",
                IntegratedSecurity = false,
                UserID = "SA",
                Password = MsSqlServerPassword,
            }.ConnectionString;

        public static async Task InitContainer()
        {
            await DockerHelper.StopAndRemoveContainerIfExistsAndNotRunningOrOld(MsSqlContainerName, oldMinutes: 60);
            await CreateAndStartMsSqlContainerIfNotExists();
            using SqlConnection connection = new(MsSqlContainerConnectionString);
            await DbHelper.WaitUntilDatabaseAvailableAsync(connection, timeoutSeconds: 60);
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
                { "1433/tcp", MsSqlServerHostPort.ToString() },
            };

            await DockerHelper.CreateAndStartContainerIfNotExists(
                MsSqlContainerName,
                $"{MsSqlImage}:{MsSqlImageTag}",
                envVariables,
                portRedirects);
        }
    }
}
