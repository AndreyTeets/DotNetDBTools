using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;

internal class PostgreSQLCheckDNDBTSysTablesExistQuery : SqlTextOnlyQuery
{
    public override string Sql =>
$@"SELECT TRUE FROM pg_catalog.pg_class WHERE relname = '{DNDBTSysTables.DNDBTDbObjects}' LIMIT 1;";
}
