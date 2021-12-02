using DotNetDBTools.Analysis.MSSQL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MSSQL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy
{
    public class MSSQLDeployManager : DeployManager
    {
        public MSSQLDeployManager() : base(
            options: new DeployOptions(),
            dbModelConverter: new MSSQLDbModelConverter(),
            factory: new MSSQLFactory())
        {
        }

        public MSSQLDeployManager(DeployOptions options) : this()
        {
            Options = options;
        }

        protected override Database CreateEmptyDatabaseModel()
        {
            return new MSSQLDatabase(null);
        }
    }
}
