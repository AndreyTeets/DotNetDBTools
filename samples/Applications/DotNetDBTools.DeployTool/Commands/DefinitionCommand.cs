using System.Data.Common;
using DotNetDBTools.Deploy;

namespace DotNetDBTools.DeployTool.Commands;

internal class DefinitionCommand : BaseCommand
{
    public void Execute(
        Dbms dbms,
        string connectionString,
        string outputPath)
    {
        IDeployManager deployManager = CreateDeployManager(dbms);
        using DbConnection connection = CreateDbConnection(dbms, connectionString);
        deployManager.GenerateDefinition(connection, outputPath);
    }
}
