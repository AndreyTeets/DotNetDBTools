using DotNetDBTools.Analysis.MySQL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MySQL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Deploy
{
    public class MySQLDeployManager : DeployManager
    {
        public MySQLDeployManager(DeployOptions options) : base(
            options: options,
            dbModelConverter: new MySQLDbModelConverter(),
            factory: new MySQLFactory())
        {
        }

        protected override Database CreateEmptyDatabaseModel()
        {
            return new MySQLDatabase(null);
        }
    }
}
