using System.Data;
using DotNetDBTools.Deploy;
using DotNetDBTools.EventsLogger;

namespace DotNetDBTools.DeployTool.Commands;

internal class RegisterCommand : BaseCommand
{
    public void Execute(
        Dbms dbms,
        string connectionString)
    {
        IDeployManager deployManager = CreateDeployManager(dbms);
        deployManager.Events.EventFired += DeployManagerEventsLogger.LogEvent;
        using IDbConnection connection = CreateDbConnection(dbms, connectionString);
        deployManager.RegisterAsDNDBT(connection);
    }
}
