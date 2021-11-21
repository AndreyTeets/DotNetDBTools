using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetDBTools.IntegrationTests.TestHelpers;
using MySqlConnector;

namespace DotNetDBTools.IntegrationTests.MySQL
{
    public class MySQLContainerHelper
    {
        private const string MySQLImage = "mysql";
        private const string MySQLImageTag = "8.0.27";
        private const string MySQLContainerName = "DotNetDBTools_IntegrationTests_MySQL";
        private const string MySQLServerPassword = "Strong(!)Passw0rd";
        private const int MySQLServerHostPort = 5006;

        public static string MySQLContainerConnectionString =>
            new MySqlConnectionStringBuilder()
            {
                Server = "localhost",
                Port = MySQLServerHostPort,
                UserID = "root",
                Password = MySQLServerPassword,
            }.ConnectionString;

        public static async Task InitContainer()
        {
            await DockerHelper.StopAndRemoveContainerIfExistsAndNotRunningOrOld(MySQLContainerName, oldMinutes: 60);
            await CreateAndStartMySQLContainerIfNotExists();
            using MySqlConnection connection = new(MySQLContainerConnectionString);
            await DbHelper.WaitUntilDatabaseAvailableAsync(connection, timeoutSeconds: 60);
        }

        private static async Task CreateAndStartMySQLContainerIfNotExists()
        {
            List<string> envVariables = new()
            {
                $"MYSQL_ROOT_PASSWORD={MySQLServerPassword}",
            };

            Dictionary<string, string> portRedirects = new()
            {
                { "3306/tcp", MySQLServerHostPort.ToString() },
            };

            await DockerHelper.CreateAndStartContainerIfNotExists(
                MySQLContainerName,
                $"{MySQLImage}:{MySQLImageTag}",
                envVariables,
                portRedirects);
        }
    }
}
