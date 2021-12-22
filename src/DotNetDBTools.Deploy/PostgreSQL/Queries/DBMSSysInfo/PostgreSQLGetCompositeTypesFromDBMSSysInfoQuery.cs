using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo
{
    internal class PostgreSQLGetCompositeTypesFromDBMSSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    t.typname AS ""{nameof(CompositeTypeRecord.TypeName)}"",
    a.attname AS ""{nameof(CompositeTypeRecord.AttributeName)}"",
    at.typname AS ""{nameof(CompositeTypeRecord.AttributeDataTypeName)}"",
    a.atttypmod AS ""{nameof(CompositeTypeRecord.AttributeDataTypeLength)}"",
    at.typtype = 'b' AS ""{nameof(CompositeTypeRecord.AttributeDataTypeIsBaseDataType)}""
FROM pg_catalog.pg_type t
INNER JOIN pg_catalog.pg_namespace n
    ON n.oid = t.typnamespace
INNER JOIN pg_catalog.pg_class c
    ON c.oid = t.typrelid
INNER JOIN pg_catalog.pg_attribute a
    ON a.attrelid = c.oid
        AND a.attnum > 0
        AND NOT a.attisdropped
INNER JOIN pg_catalog.pg_type at
    ON at.oid = a.atttypid
WHERE t.typtype = 'c'
    AND c.relkind = 'c'
    AND n.nspname NOT IN ('information_schema', 'pg_catalog');";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        public class CompositeTypeRecord
        {
            public string TypeName { get; set; }
            public string AttributeName { get; set; }
            public string AttributeDataTypeName { get; set; }
            public string AttributeDataTypeLength { get; set; }
            public bool AttributeDataTypeIsBaseDataType { get; set; }
        }
    }
}
