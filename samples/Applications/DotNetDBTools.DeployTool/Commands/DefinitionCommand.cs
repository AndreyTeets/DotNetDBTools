using System.Data.Common;
using DotNetDBTools.Deploy;
using DotNetDBTools.EventsLogger;

namespace DotNetDBTools.DeployTool.Commands;

internal class DefinitionCommand : BaseCommand
{
    public void Execute(
        Dbms dbms,
        string connectionString,
        string outputPath)
    {
        IDeployManager deployManager = CreateDeployManager(dbms);
        deployManager.Events.EventFired += DeployManagerEventsLogger.LogEvent;
        using DbConnection connection = CreateDbConnection(dbms, connectionString);
        deployManager.GenerateDefinition(connection, outputPath);
    }
}
