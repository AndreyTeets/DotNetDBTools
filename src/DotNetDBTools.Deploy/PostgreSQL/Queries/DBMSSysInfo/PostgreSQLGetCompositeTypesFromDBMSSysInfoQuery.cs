using DotNetDBTools.Deploy.Core.Queries;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo;

internal class PostgreSQLGetCompositeTypesFromDBMSSysInfoQuery : NoParametersQuery
{
    public override string Sql =>
$@"SELECT
    t.typname AS ""{nameof(CompositeTypeRecord.TypeName)}"",
    a.attname AS ""{nameof(CompositeTypeRecord.AttributeName)}"",
    at.typname AS ""{nameof(CompositeTypeRecord.AttributeDataTypeName)}"",
    a.atttypmod AS ""{nameof(CompositeTypeRecord.AttributeDataTypeLength)}"",
    a.attndims AS ""{nameof(CompositeTypeRecord.AttributeArrayDims)}"",
    aet.typname AS ""{nameof(CompositeTypeRecord.AttributeArrayElemDataType)}""
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
LEFT JOIN pg_catalog.pg_type aet
    ON aet.oid = at.typelem
WHERE t.typtype = 'c'
    AND c.relkind = 'c'
    AND n.nspname NOT IN ('information_schema', 'pg_catalog');";

    public class CompositeTypeRecord
    {
        public string TypeName { get; set; }
        public string AttributeName { get; set; }
        public string AttributeDataTypeName { get; set; }
        public string AttributeDataTypeLength { get; set; }
        public int AttributeArrayDims { get; set; }
        public string AttributeArrayElemDataType { get; set; }
    }
}
