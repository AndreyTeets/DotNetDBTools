using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core;

internal interface IDbModelFromDbSysInfoBuilder
{
    public Database GetDatabaseModelFromDNDBTSysInfo();
    public Database GenerateDatabaseModelFromDBMSSysInfo();
}
