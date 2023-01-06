using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;

internal class PostgreSQLCheckDNDBTSysTablesExistQuery : NoParametersQuery
{
    public override string Sql =>
$@"SELECT TRUE FROM pg_catalog.pg_class WHERE relname IN ({DNDBTSysTables.AllTablesForInClause}) LIMIT 1;";
}
