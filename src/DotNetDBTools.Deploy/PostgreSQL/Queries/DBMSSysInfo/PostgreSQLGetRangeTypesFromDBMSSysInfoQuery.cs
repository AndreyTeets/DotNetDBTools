using DotNetDBTools.Deploy.Core.Queries;
using QH = DotNetDBTools.Deploy.PostgreSQL.PostgreSQLQueriesHelper;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo;

internal class PostgreSQLGetRangeTypesFromDBMSSysInfoQuery : NoParametersQuery
{
    public override string Sql =>
$@"SELECT
    t.typname AS ""{nameof(RangeTypeRecord.TypeName)}"",
    rst.typname AS ""{nameof(RangeTypeRecord.SubtypeName)}"",
    oc.opcname AS ""{nameof(RangeTypeRecord.SubtypeOperatorClass)}"",
    cl.collname AS ""{nameof(RangeTypeRecord.Collation)}"",
    r.rngcanonical::text AS ""{nameof(RangeTypeRecord.CanonicalFunction)}"",
    r.rngsubdiff::text AS ""{nameof(RangeTypeRecord.SubtypeDiff)}"",
    {GetMultirangeTypeNameSelectResultStatement()} AS ""{nameof(RangeTypeRecord.MultirangeTypeName)}"",
    arst.typname AS ""{nameof(RangeTypeRecord.SubtypeArrayElemDataType)}""
FROM pg_catalog.pg_type t
INNER JOIN pg_catalog.pg_namespace n
    ON n.oid = t.typnamespace
INNER JOIN pg_catalog.pg_range r
    ON r.rngtypid = t.oid
INNER JOIN pg_catalog.pg_type rst
    ON rst.oid = r.rngsubtype{GetMultirangeTypeNameExtraJoinStatement()}
LEFT JOIN pg_catalog.pg_type arst
    ON arst.oid = rst.typelem
INNER JOIN pg_catalog.pg_opclass oc
    ON oc.oid = r.rngsubopc
LEFT JOIN pg_catalog.pg_collation cl
    ON cl.oid = r.rngcollation
WHERE t.typtype = 'r'
    AND n.nspname NOT IN ('information_schema', 'pg_catalog');";

    private readonly int _dbmsVersion;

    public PostgreSQLGetRangeTypesFromDBMSSysInfoQuery(int dbmsVersion)
    {
        _dbmsVersion = dbmsVersion;
    }

    private string GetMultirangeTypeNameSelectResultStatement()
    {
        if (_dbmsVersion >= QH.MultirangeTypeNameAvailableDbmsVersion)
            return "rmt.typname";
        else
            return "t.typname || '_multirange'";
    }

    private string GetMultirangeTypeNameExtraJoinStatement()
    {
        if (_dbmsVersion >= QH.MultirangeTypeNameAvailableDbmsVersion)
        {
            return
@"
INNER JOIN pg_catalog.pg_type rmt
    ON rmt.oid = r.rngmultitypid";
        }
        else
        {
            return "";
        }
    }

    public class RangeTypeRecord
    {
        public string TypeName { get; set; }
        public string SubtypeName { get; set; }
        public string SubtypeOperatorClass { get; set; }
        public string Collation { get; set; }
        public string CanonicalFunction { get; set; }
        public string SubtypeDiff { get; set; }
        public string MultirangeTypeName { get; set; }
        public string SubtypeArrayElemDataType { get; set; }
    }
}
