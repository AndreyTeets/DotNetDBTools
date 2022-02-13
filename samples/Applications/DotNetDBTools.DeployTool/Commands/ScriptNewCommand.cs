using DotNetDBTools.Deploy;

namespace DotNetDBTools.DeployTool.Commands;

internal class ScriptNewCommand : BaseCommand
{
    public void Execute(
        Dbms dbms,
        string dbAssemblyPath,
        string outputPath,
        bool ddlOnly)
    {
        IDeployManager deployManager = CreateDeployManager(dbms);
        if (ddlOnly)
            deployManager.GenerateDDLOnlyPublishScript(dbAssemblyPath, outputPath);
        else
            deployManager.GeneratePublishScript(dbAssemblyPath, outputPath);
    }
}
