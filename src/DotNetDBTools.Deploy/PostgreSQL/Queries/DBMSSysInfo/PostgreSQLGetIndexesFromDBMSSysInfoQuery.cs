using System;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo;

internal class PostgreSQLGetIndexesFromDBMSSysInfoQuery : GetIndexesFromDBMSSysInfoQuery
{
    public override string Sql =>
$@"SELECT
    t.relname AS ""{nameof(IndexRecord.TableName)}"",
    c.relname AS ""{nameof(IndexRecord.IndexName)}"",
    i.indisunique AS ""{nameof(IndexRecord.IsUnique)}"",
    col.attname AS ""{nameof(IndexRecord.ColumnName)}"",
    CASE WHEN col_map.col_pos > i.indnkeyatts
        THEN TRUE
        ELSE FALSE
    END AS ""{nameof(IndexRecord.IsIncludeColumn)}"",
    CASE WHEN col_map.col_pos > i.indnkeyatts
        THEN col_map.col_pos - i.indnkeyatts
        ELSE col_map.col_pos
    END AS ""{nameof(IndexRecord.ColumnPosition)}""
FROM pg_index i
INNER JOIN pg_class c
    ON c.oid = indexrelid
INNER JOIN pg_class t
    ON t.oid = i.indrelid
INNER JOIN pg_catalog.pg_namespace n
    ON n.oid = t.relnamespace
INNER JOIN LATERAL UNNEST(i.indkey) WITH ORDINALITY col_map(col_num, col_pos)
    ON TRUE
INNER JOIN pg_catalog.pg_attribute col
    ON col.attrelid = t.oid
        AND col.attnum = col_map.col_num
        AND NOT col.attisdropped
LEFT JOIN pg_catalog.pg_constraint pc
    ON pc.conindid = i.indexrelid
WHERE n.nspname NOT IN ('information_schema', 'pg_catalog', 'pg_toast')
    AND i.indisprimary = FALSE
    AND pc.oid IS NULL
    AND t.relname NOT IN ({DNDBTSysTables.AllTablesForInClause});";

    public override RecordMapper Mapper => new PostgreSQLRecordMapper();

    public class PostgreSQLRecordMapper : RecordMapper
    {
        public override Index MapExceptColumnsToIndexModel(IndexRecord indexRecord)
        {
            return new PostgreSQLIndex()
            {
                ID = Guid.NewGuid(),
                Name = indexRecord.IndexName,
                Unique = indexRecord.IsUnique,
            };
        }
    }
}
