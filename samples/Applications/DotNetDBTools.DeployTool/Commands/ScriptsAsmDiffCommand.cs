using DotNetDBTools.Deploy;
using DotNetDBTools.EventsLogger;

namespace DotNetDBTools.DeployTool.Commands;

internal class ScriptsAsmDiffCommand : BaseCommand
{
    public void Execute(
        Dbms dbms,
        string newDbAssemblyPath,
        string oldDbAssemblyPath,
        string outputPath,
        bool allowDataLoss,
        bool noDNDBTInfo)
    {
        IDeployManager deployManager = CreateDeployManager(dbms);
        deployManager.Options.AllowDataLoss = allowDataLoss;
        deployManager.Events.EventFired += DeployManagerEventsLogger.LogEvent;
        if (noDNDBTInfo)
            deployManager.GenerateNoDNDBTInfoPublishScript(newDbAssemblyPath, oldDbAssemblyPath, outputPath);
        else
            deployManager.GeneratePublishScript(newDbAssemblyPath, oldDbAssemblyPath, outputPath);
    }
}
