using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetDBTools.IntegrationTests.Utilities;
using Npgsql;

namespace DotNetDBTools.IntegrationTests.PostgreSQL;

internal class PostgreSQLContainerHelper
{
    private const string PostgreSQLImage = "docker.io/library/postgres";
    private const string PostgreSQLContainerName = "DotNetDBTools_IntegrationTests_PostgreSQL";
    private const string PostgreSQLServerPassword = "Strong(!)Passw0rd";
    private const int PostgreSQLServerHostPort = 5007;

    private static string PostgreSQLImageTag =>
        Environment.GetEnvironmentVariable("USE_LATEST_DBMS_VERSION") != "true"
        ? "15.1-alpine3.17"
        : "15.1-alpine3.17";

    public static string PostgreSQLContainerConnectionString =>
        new NpgsqlConnectionStringBuilder()
        {
            Host = "127.0.0.1",
            Port = PostgreSQLServerHostPort,
            Username = "postgres",
            Password = PostgreSQLServerPassword,
        }.ConnectionString;

    public static async Task InitContainer()
    {
        await DockerRunner.StopAndRemoveContainerIfExistsAndNotRunningOrOld(PostgreSQLContainerName, oldMinutes: 60);
        await CreateAndStartPostgreSQLContainerIfNotExists();
        using NpgsqlConnection connection = new(PostgreSQLContainerConnectionString);
        await DbAvailabilityChecker.WaitUntilDatabaseAvailableAsync(connection, timeoutSeconds: 90);
    }

    private static async Task CreateAndStartPostgreSQLContainerIfNotExists()
    {
        List<string> envVariables = new()
        {
            $"POSTGRES_PASSWORD={PostgreSQLServerPassword}",
        };

        Dictionary<string, string> portRedirects = new()
        {
            { "5432/tcp", PostgreSQLServerHostPort.ToString() },
        };

        await DockerRunner.CreateAndStartContainerIfNotExists(
            PostgreSQLContainerName,
            $"{PostgreSQLImage}:{PostgreSQLImageTag}",
            envVariables,
            portRedirects);
    }
}
