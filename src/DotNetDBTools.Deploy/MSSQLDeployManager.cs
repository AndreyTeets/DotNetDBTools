using DotNetDBTools.Analysis.MSSQL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MSSQL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy
{
    public class MSSQLDeployManager : DeployManager
    {
        public MSSQLDeployManager(DeployOptions options) : base(
            options: options,
            dbModelConverter: new MSSQLDbModelConverter(),
            factory: new MSSQLFactory())
        {
        }

        protected override Database CreateEmptyDatabaseModel()
        {
            return new MSSQLDatabase(null);
        }
    }
}
