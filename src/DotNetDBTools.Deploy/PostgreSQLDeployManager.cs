using DotNetDBTools.Analysis.PostgreSQL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.PostgreSQL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Deploy
{
    public class PostgreSQLDeployManager : DeployManager
    {
        public PostgreSQLDeployManager() : base(
            options: new DeployOptions(),
            dbModelConverter: new PostgreSQLDbModelConverter(),
            factory: new PostgreSQLFactory())
        {
        }

        public PostgreSQLDeployManager(DeployOptions options) : this()
        {
            Options = options;
        }

        protected override Database CreateEmptyDatabaseModel()
        {
            return new PostgreSQLDatabase(null);
        }
    }
}
