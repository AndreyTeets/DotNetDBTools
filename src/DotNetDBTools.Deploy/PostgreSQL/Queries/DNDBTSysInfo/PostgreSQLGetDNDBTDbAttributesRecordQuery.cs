using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;

internal class PostgreSQLGetDNDBTDbAttributesRecordQuery : GetDNDBTDbAttributesRecordQuery
{
    public override string Sql =>
$@"SELECT
    ""{DNDBTSysTables.DNDBTDbAttributes.Version}""
FROM ""{DNDBTSysTables.DNDBTDbAttributes}"";";
}
