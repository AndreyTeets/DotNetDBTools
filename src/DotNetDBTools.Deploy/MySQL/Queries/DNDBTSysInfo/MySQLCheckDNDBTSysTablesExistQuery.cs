using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries;

namespace DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo;

internal class MySQLCheckDNDBTSysTablesExistQuery : NoParametersQuery
{
    public override string Sql =>
$@"SELECT TRUE FROM information_schema.tables WHERE TABLE_SCHEMA = (SELECT DATABASE()) AND TABLE_NAME IN ({DNDBTSysTables.AllTablesForInClause}) LIMIT 1;";
}
