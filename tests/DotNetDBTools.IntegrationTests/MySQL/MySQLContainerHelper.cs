﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetDBTools.IntegrationTests.Utilities;
using MySqlConnector;

namespace DotNetDBTools.IntegrationTests.MySQL;

internal class MySQLContainerHelper
{
    private const string MySQLImage = "docker.io/library/mysql";
    private const string MySQLContainerName = "DotNetDBTools_IntegrationTests_MySQL";
    private const string MySQLServerPassword = "Strong(!)Passw0rd";
    private const int MySQLServerHostPort = 5006;

    private static string MySQLImageTag =>
        Environment.GetEnvironmentVariable("DNDBT_USE_LATEST_DBMS_VERSION") != "true"
        ? "8.0.19"
        : "8.0.31";

    public static string MySQLContainerConnectionString =>
        new MySqlConnectionStringBuilder()
        {
            Server = "127.0.0.1",
            Port = MySQLServerHostPort,
            UserID = "root",
            Password = MySQLServerPassword,
        }.ConnectionString;

    public static async Task InitContainer()
    {
        await DockerRunner.StopAndRemoveContainerIfExistsAndNotRunningOrOld(MySQLContainerName, oldMinutes: 60);
        await CreateAndStartMySQLContainerIfNotExists();
        using MySqlConnection connection = new(MySQLContainerConnectionString);
        await DbAvailabilityChecker.WaitUntilDatabaseAvailableAsync(connection, timeoutSeconds: 90);
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

        await DockerRunner.CreateAndStartContainerIfNotExists(
            MySQLContainerName,
            $"{MySQLImage}:{MySQLImageTag}",
            envVariables,
            portRedirects);
    }
}
