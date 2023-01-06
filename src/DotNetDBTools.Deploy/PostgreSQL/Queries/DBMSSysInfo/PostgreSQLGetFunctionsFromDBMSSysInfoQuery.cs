using DotNetDBTools.Deploy.Core.Queries;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo;

internal class PostgreSQLGetFunctionsFromDBMSSysInfoQuery : NoParametersQuery
{
    public override string Sql =>
$@"SELECT
    p.proname AS ""{nameof(FunctionRecord.FunctionName)}"",
    pg_catalog.pg_get_functiondef(p.oid) ""{nameof(FunctionRecord.FunctionCode)}""
FROM pg_catalog.pg_proc p
INNER JOIN pg_catalog.pg_namespace n
    ON n.oid = p.pronamespace
INNER JOIN pg_catalog.pg_language l
    ON l.oid = p.prolang
WHERE p.prokind = 'f'
    AND l.lanname IN ('sql', 'plpgsql')
    AND n.nspname NOT IN ('information_schema', 'pg_catalog');";

    public class FunctionRecord
    {
        public string FunctionName { get; set; }
        public string FunctionCode { get; set; }
    }
}
