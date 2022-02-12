using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.SQLite;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Deploy;

public class SQLiteDeployManager : DeployManager<SQLiteDatabase>
{
    public SQLiteDeployManager()
        : this(new DeployOptions()) { }

    public SQLiteDeployManager(DeployOptions options) : base(
        options: new DeployOptions(),
        factory: new SQLiteFactory())
    {
        Options = options;
    }
}
