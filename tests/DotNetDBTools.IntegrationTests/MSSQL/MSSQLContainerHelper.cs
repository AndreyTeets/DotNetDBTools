﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetDBTools.IntegrationTests.Utilities;
using Microsoft.Data.SqlClient;

namespace DotNetDBTools.IntegrationTests.MSSQL;

internal class MSSQLContainerHelper
{
    private const string MSSQLImage = "mcr.microsoft.com/mssql/server";
    private const string MSSQLContainerName = "DotNetDBTools_IntegrationTests_MSSQL";
    private const string MSSQLServerPassword = "Strong(!)Passw0rd";
    private const int MSSQLServerHostPort = 5005;

    private static string MSSQLImageTag =>
        Environment.GetEnvironmentVariable("DNDBT_USE_LATEST_DBMS_VERSION") != "true"
        ? "2017-latest"
        : "2022-latest";

    public static string MSSQLContainerConnectionString =>
        new SqlConnectionStringBuilder()
        {
            DataSource = $"127.0.0.1,{MSSQLServerHostPort}",
            IntegratedSecurity = false,
            UserID = "SA",
            Password = MSSQLServerPassword,
            TrustServerCertificate = true,
        }.ConnectionString;

    public static async Task InitContainer()
    {
        await DockerRunner.StopAndRemoveContainerIfExistsAndNotRunningOrOld(MSSQLContainerName, oldMinutes: 60);
        await CreateAndStartMsSqlContainerIfNotExists();
        using SqlConnection connection = new(MSSQLContainerConnectionString);
        await DbAvailabilityChecker.WaitUntilDatabaseAvailableAsync(connection, timeoutSeconds: 90);
    }

    private static async Task CreateAndStartMsSqlContainerIfNotExists()
    {
        List<string> envVariables = new()
        {
            "ACCEPT_EULA=Y",
            $"SA_PASSWORD={MSSQLServerPassword}",
        };

        Dictionary<string, string> portRedirects = new()
        {
            { "1433/tcp", MSSQLServerHostPort.ToString() },
        };

        await DockerRunner.CreateAndStartContainerIfNotExists(
            MSSQLContainerName,
            $"{MSSQLImage}:{MSSQLImageTag}",
            envVariables,
            portRedirects);
    }
}
