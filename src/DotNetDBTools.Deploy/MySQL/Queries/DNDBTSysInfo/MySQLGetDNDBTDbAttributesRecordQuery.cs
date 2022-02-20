using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo;

internal class MySQLGetDNDBTDbAttributesRecordQuery : GetDNDBTDbAttributesRecordQuery
{
    public override string Sql =>
$@"SELECT
    `{DNDBTSysTables.DNDBTDbAttributes.Version}`
FROM `{DNDBTSysTables.DNDBTDbAttributes}`;";
}
