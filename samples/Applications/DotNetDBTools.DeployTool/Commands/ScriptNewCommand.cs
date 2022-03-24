using DotNetDBTools.Deploy;
using DotNetDBTools.EventsLogger;

namespace DotNetDBTools.DeployTool.Commands;

internal class ScriptNewCommand : BaseCommand
{
    public void Execute(
        Dbms dbms,
        string dbAssemblyPath,
        string outputPath,
        bool noDNDBTInfo)
    {
        IDeployManager deployManager = CreateDeployManager(dbms);
        deployManager.Events.EventFired += DeployManagerEventsLogger.LogEvent;
        if (noDNDBTInfo)
            deployManager.GenerateNoDNDBTInfoPublishScript(dbAssemblyPath, outputPath);
        else
            deployManager.GeneratePublishScript(dbAssemblyPath, outputPath);
    }
}
