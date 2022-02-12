using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Deploy;

public class PostgreSQLDeployManager : DeployManager<PostgreSQLDatabase>
{
    public PostgreSQLDeployManager()
        : this(new DeployOptions()) { }

    public PostgreSQLDeployManager(DeployOptions options) : base(
        options: new DeployOptions(),
        factory: new PostgreSQLFactory())
    {
        Options = options;
    }
}
