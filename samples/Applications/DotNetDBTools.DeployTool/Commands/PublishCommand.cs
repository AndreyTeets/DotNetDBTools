using System.Data;
using DotNetDBTools.Deploy;
using DotNetDBTools.EventsLogger;

namespace DotNetDBTools.DeployTool.Commands;

internal class PublishCommand : BaseCommand
{
    public void Execute(
        Dbms dbms,
        string dbAssemblyPath,
        string connectionString,
        bool allowDataLoss)
    {
        IDeployManager deployManager = CreateDeployManager(dbms);
        deployManager.Options.AllowDataLoss = allowDataLoss;
        deployManager.Events.EventFired += DeployManagerEventsLogger.LogEvent;
        using IDbConnection connection = CreateDbConnection(dbms, connectionString);
        deployManager.PublishDatabase(dbAssemblyPath, connection);
    }
}
