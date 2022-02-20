using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;

internal class SQLiteGetDNDBTDbAttributesRecordQuery : GetDNDBTDbAttributesRecordQuery
{
    public override string Sql =>
$@"SELECT
    [{DNDBTSysTables.DNDBTDbAttributes.Version}]
FROM [{DNDBTSysTables.DNDBTDbAttributes}];";
}
