using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DBMSSysInfo;

internal class MSSQLGetViewsFromDBMSSysInfoQuery : GetViewsFromDBMSSysInfoQuery
{
    public override string Sql =>
$@"SELECT
    o.Name AS [{nameof(ViewRecord.ViewName)}],
    m.definition AS [{nameof(ViewRecord.ViewCode)}]
FROM sys.objects o
INNER JOIN sys.sql_modules m
    ON m.object_id = o.object_id
WHERE o.type = 'V'";
}
