using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MSSQL;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy;

public class MSSQLDeployManager : DeployManager<MSSQLDatabase>
{
    public MSSQLDeployManager()
        : this(new DeployOptions()) { }

    public MSSQLDeployManager(DeployOptions options) : base(
        options: new DeployOptions(),
        factory: new MSSQLFactory())
    {
        Options = options;
    }
}
