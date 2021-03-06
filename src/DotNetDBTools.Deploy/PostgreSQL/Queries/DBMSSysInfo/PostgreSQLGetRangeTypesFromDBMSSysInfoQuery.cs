using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo;

internal class PostgreSQLGetRangeTypesFromDBMSSysInfoQuery : IQuery
{
    public string Sql =>
$@"SELECT
    t.typname AS ""{nameof(RangeTypeRecord.TypeName)}"",
    rst.typname AS ""{nameof(RangeTypeRecord.SubtypeName)}"",
    rst.typtype = 'b' AS ""{nameof(RangeTypeRecord.SubtypeIsBaseDataType)}"",
    oc.opcname AS ""{nameof(RangeTypeRecord.SubtypeOperatorClass)}"",
    cl.collname AS ""{nameof(RangeTypeRecord.Collation)}"",
    r.rngcanonical::text AS ""{nameof(RangeTypeRecord.CanonicalFunction)}"",
    r.rngsubdiff::text AS ""{nameof(RangeTypeRecord.SubtypeDiff)}"",
    rmt.typname AS ""{nameof(RangeTypeRecord.MultirangeTypeName)}""
FROM pg_catalog.pg_type t
INNER JOIN pg_catalog.pg_namespace n
    ON n.oid = t.typnamespace
INNER JOIN pg_catalog.pg_range r
    ON r.rngtypid = t.oid
INNER JOIN pg_catalog.pg_type rst
    ON rst.oid = r.rngsubtype
INNER JOIN pg_catalog.pg_type rmt
    ON rmt.oid = r.rngmultitypid
INNER JOIN pg_catalog.pg_opclass oc
    ON oc.oid = r.rngsubopc
LEFT JOIN pg_catalog.pg_collation cl
    ON cl.oid = r.rngcollation
WHERE t.typtype = 'r'
    AND n.nspname NOT IN ('information_schema', 'pg_catalog');";

    public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

    public class RangeTypeRecord
    {
        public string TypeName { get; set; }
        public string SubtypeName { get; set; }
        public bool SubtypeIsBaseDataType { get; set; }
        public string SubtypeOperatorClass { get; set; }
        public string Collation { get; set; }
        public string CanonicalFunction { get; set; }
        public string SubtypeDiff { get; set; }
        public string MultirangeTypeName { get; set; }
    }
}
