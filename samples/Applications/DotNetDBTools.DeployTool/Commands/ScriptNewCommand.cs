using DotNetDBTools.Deploy;

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
        if (noDNDBTInfo)
            deployManager.GenerateNoDNDBTInfoPublishScript(dbAssemblyPath, outputPath);
        else
            deployManager.GeneratePublishScript(dbAssemblyPath, outputPath);
    }
}
