using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;

namespace DotNetDBTools.Deploy.MySQL.Queries.DBMSSysInfo;

internal class MySQLGetViewsFromDBMSSysInfoQuery : GetViewsFromDBMSSysInfoQuery
{
    public override string Sql =>
$@"SELECT
    v.TABLE_NAME AS `{nameof(ViewRecord.ViewName)}`,
    CONCAT('create view `', v.TABLE_NAME, '` as ', v.VIEW_DEFINITION) AS `{nameof(ViewRecord.ViewCode)}`
FROM INFORMATION_SCHEMA.VIEWS v
WHERE v.TABLE_SCHEMA = (select DATABASE());";
}
