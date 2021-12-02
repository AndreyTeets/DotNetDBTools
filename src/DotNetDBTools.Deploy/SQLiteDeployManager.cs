using DotNetDBTools.Analysis.SQLite;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.SQLite;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Deploy
{
    public class SQLiteDeployManager : DeployManager
    {
        public SQLiteDeployManager() : base(
            options: new DeployOptions(),
            dbModelConverter: new SQLiteDbModelConverter(),
            factory: new SQLiteFactory())
        {
        }

        public SQLiteDeployManager(DeployOptions options) : this()
        {
            Options = options;
        }

        protected override Database CreateEmptyDatabaseModel()
        {
            return new SQLiteDatabase(null);
        }
    }
}
