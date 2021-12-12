using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries;

namespace DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo
{
    internal class SQLiteCheckDNDBTSysTablesExistQuery : SqlTextOnlyQuery
    {
        public override string Sql =>
$@"SELECT
    true
FROM sqlite_master
WHERE type = 'table' AND name = '{DNDBTSysTables.DNDBTDbObjects}';";
    }
}
