using DotNetDBTools.Analysis.SQLite;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.SQLite;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Deploy
{
    public class SQLiteDeployManager : DeployManager
    {
        public SQLiteDeployManager(DeployOptions options) : base(
            options: options,
            dbModelConverter: new SQLiteDbModelConverter(),
            factory: new SQLiteFactory())
        {
        }

        protected override Database CreateEmptyDatabaseModel()
        {
            return new SQLiteDatabase(null);
        }
    }
}
