using DotNetDBTools.Deploy;

namespace DotNetDBTools.DeployTool.Commands;

internal class ScriptNewCommand : BaseCommand
{
    public void Execute(
        Dbms dbms,
        string dbAssemblyPath,
        string outputPath)
    {
        IDeployManager deployManager = CreateDeployManager(dbms);
        deployManager.GeneratePublishScript(dbAssemblyPath, outputPath);
    }
}
