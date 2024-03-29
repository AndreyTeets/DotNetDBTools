﻿using System.Data;
using DotNetDBTools.Deploy;
using DotNetDBTools.EventsLogger;

namespace DotNetDBTools.DeployTool.Commands;

internal class ScriptUpdateCommand : BaseCommand
{
    public void Execute(
        Dbms dbms,
        string dbAssemblyPath,
        string connectionString,
        string outputPath,
        bool allowDataLoss)
    {
        IDeployManager deployManager = CreateDeployManager(dbms);
        deployManager.Options.AllowDataLoss = allowDataLoss;
        deployManager.Events.EventFired += DeployManagerEventsLogger.LogEvent;
        using IDbConnection connection = CreateDbConnection(dbms, connectionString);
        string script = deployManager.GeneratePublishScript(dbAssemblyPath, connection);
        SaveToFile(outputPath, script);
    }
}
