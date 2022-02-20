using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo;

internal class MSSQLGetDNDBTDbAttributesRecordQuery : GetDNDBTDbAttributesRecordQuery
{
    public override string Sql =>
$@"SELECT
    [{DNDBTSysTables.DNDBTDbAttributes.Version}]
FROM [{DNDBTSysTables.DNDBTDbAttributes}];";
}
