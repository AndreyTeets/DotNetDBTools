using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries;

namespace DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;

internal class SQLiteDropDNDBTSysTablesQuery : SqlTextOnlyQuery
{
    public override string Sql =>
$@"DROP TABLE {DNDBTSysTables.DNDBTDbObjects};";
}
