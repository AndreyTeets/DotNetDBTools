using DotNetDBTools.Analysis.PostgreSQL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.PostgreSQL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Deploy
{
    public class PostgreSQLDeployManager : DeployManager
    {
        public PostgreSQLDeployManager(DeployOptions options) : base(
            options: options,
            dbModelConverter: new PostgreSQLDbModelConverter(),
            factory: new PostgreSQLFactory())
        {
        }

        protected override Database CreateEmptyDatabaseModel()
        {
            return new PostgreSQLDatabase(null);
        }
    }
}
