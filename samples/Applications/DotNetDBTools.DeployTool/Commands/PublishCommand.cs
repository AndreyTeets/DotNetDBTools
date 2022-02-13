using System.Data.Common;
using DotNetDBTools.Deploy;

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
        using DbConnection connection = CreateDbConnection(dbms, connectionString);
        deployManager.PublishDatabase(dbAssemblyPath, connection);
    }
}
