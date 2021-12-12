using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MySQL;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Deploy
{
    public class MySQLDeployManager : DeployManager<MySQLDatabase>
    {
        public MySQLDeployManager()
            : this(new DeployOptions()) { }

        public MySQLDeployManager(DeployOptions options) : base(
            options: new DeployOptions(),
            factory: new MySQLFactory())
        {
            Options = options;
        }
    }
}
