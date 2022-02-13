using System.Data.Common;
using DotNetDBTools.Deploy;

namespace DotNetDBTools.DeployTool.Commands;

internal class UnregisterCommand : BaseCommand
{
    public void Execute(
        Dbms dbms,
        string connectionString)
    {
        IDeployManager deployManager = CreateDeployManager(dbms);
        using DbConnection connection = CreateDbConnection(dbms, connectionString);
        deployManager.UnregisterAsDNDBT(connection);
    }
}
