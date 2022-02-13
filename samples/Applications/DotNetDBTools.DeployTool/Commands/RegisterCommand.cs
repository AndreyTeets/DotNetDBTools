using System.Data.Common;
using DotNetDBTools.Deploy;

namespace DotNetDBTools.DeployTool.Commands;

internal class RegisterCommand : BaseCommand
{
    public void Execute(
        Dbms dbms,
        string connectionString)
    {
        IDeployManager deployManager = CreateDeployManager(dbms);
        using DbConnection connection = CreateDbConnection(dbms, connectionString);
        deployManager.RegisterAsDNDBT(connection);
    }
}
