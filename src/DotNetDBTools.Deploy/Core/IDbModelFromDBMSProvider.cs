using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core;

internal interface IDbModelFromDBMSProvider
{
    public Database CreateDbModelUsingDNDBTSysInfo();
    public Database CreateDbModelUsingDBMSSysInfo();
}
