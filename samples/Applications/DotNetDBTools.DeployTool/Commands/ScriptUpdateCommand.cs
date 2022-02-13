using System.Data.Common;
using DotNetDBTools.Deploy;

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
        using DbConnection connection = CreateDbConnection(dbms, connectionString);
        deployManager.GeneratePublishScript(dbAssemblyPath, connection, outputPath);
    }
}
