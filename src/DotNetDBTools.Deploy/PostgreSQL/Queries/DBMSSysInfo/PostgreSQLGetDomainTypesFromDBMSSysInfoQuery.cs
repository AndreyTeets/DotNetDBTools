using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo
{
    internal class PostgreSQLGetDomainTypesFromDBMSSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    t.typname AS ""{nameof(DomainTypeRecord.TypeName)}"",
    bt.typname AS ""{nameof(DomainTypeRecord.UnderlyingTypeName)}"",
    t.typtypmod AS ""{nameof(DomainTypeRecord.UnderlyingTypeLength)}"",
    bt.typtype = 'b' AS ""{nameof(DomainTypeRecord.UnderlyingTypeIsBaseDataType)}"",
    t.typdefault AS ""{nameof(DomainTypeRecord.Default)}"",
    NOT t.typnotnull AS ""{nameof(DomainTypeRecord.Nullable)}"",
    c.con_name AS ""{nameof(DomainTypeRecord.CheckConstrantName)}"",
    c.con_def AS ""{nameof(DomainTypeRecord.CheckConstrantCode)}""
FROM pg_catalog.pg_type t
INNER JOIN pg_catalog.pg_namespace n
    ON n.oid = t.typnamespace
INNER JOIN pg_catalog.pg_type bt
    ON bt.oid = t.typbasetype
LEFT JOIN LATERAL (
    SELECT
        x.conname AS con_name,
        pg_catalog.pg_get_constraintdef(x.oid, TRUE) AS con_def
    FROM pg_catalog.pg_constraint x
    WHERE x.contypid = t.oid
) c(con_name, con_def)
    ON TRUE
WHERE t.typtype = 'd'
    AND n.nspname NOT IN ('information_schema', 'pg_catalog');";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        public class DomainTypeRecord
        {
            public string TypeName { get; set; }
            public string UnderlyingTypeName { get; set; }
            public string UnderlyingTypeLength { get; set; }
            public bool UnderlyingTypeIsBaseDataType { get; set; }
            public string Default { get; set; }
            public bool Nullable { get; set; }
            public string CheckConstrantName { get; set; }
            public string CheckConstrantCode { get; set; }
        }
    }
}
