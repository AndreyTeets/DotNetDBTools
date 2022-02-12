using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;

namespace DotNetDBTools.Deploy.SQLite.Queries.DBMSSysInfo;

internal class SQLiteGetViewsFromDBMSSysInfoQuery : GetViewsFromDBMSSysInfoQuery
{
    public override string Sql =>
$@"SELECT
    sm.name AS {nameof(ViewRecord.ViewName)},
    sm.sql AS {nameof(ViewRecord.ViewCode)}
FROM sqlite_master sm
WHERE sm.type = 'view';";
}
