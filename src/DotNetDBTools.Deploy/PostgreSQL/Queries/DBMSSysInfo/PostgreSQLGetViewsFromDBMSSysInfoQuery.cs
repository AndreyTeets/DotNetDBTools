using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo
{
    internal class PostgreSQLGetViewsFromDBMSSysInfoQuery : GetViewsFromDBMSSysInfoQuery
    {
        public override string Sql =>
$@"SELECT
    c.relname AS ""{nameof(ViewRecord.ViewName)}"",
    'CREATE VIEW ""' || c.relname || '"" AS' || pg_catalog.pg_get_viewdef(c.oid, true) AS ""{nameof(ViewRecord.ViewCode)}""
FROM pg_catalog.pg_class c
INNER JOIN pg_catalog.pg_namespace n
    ON n.oid = c.relnamespace
WHERE c.relkind = 'v'
    AND n.nspname NOT IN ('information_schema', 'pg_catalog');";
    }
}
