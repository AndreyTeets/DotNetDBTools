using DotNetDBTools.Analysis.MySQL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MySQL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Deploy
{
    public class MySQLDeployManager : DeployManager
    {
        public MySQLDeployManager() : base(
            options: new DeployOptions(),
            dbModelConverter: new MySQLDbModelConverter(),
            factory: new MySQLFactory())
        {
        }

        public MySQLDeployManager(DeployOptions options) : this()
        {
            Options = options;
        }

        protected override Database CreateEmptyDatabaseModel()
        {
            return new MySQLDatabase(null);
        }
    }
}
