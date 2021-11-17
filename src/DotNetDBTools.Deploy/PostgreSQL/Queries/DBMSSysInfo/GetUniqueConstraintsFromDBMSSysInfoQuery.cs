using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.ModelBuilders;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo
{
    internal class GetUniqueConstraintsFromDBMSSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    t.relname AS ""{nameof(UniqueConstraintRecord.TableName)}"",
    c.conname AS ""{nameof(UniqueConstraintRecord.ConstraintName)}"",
    a.attname AS ""{nameof(UniqueConstraintRecord.ColumnName)}"",
    p.col_pos AS ""{nameof(UniqueConstraintRecord.ColumnPosition)}""
FROM pg_catalog.pg_class t
INNER JOIN pg_catalog.pg_namespace n
    ON n.oid = t.relnamespace
INNER JOIN pg_catalog.pg_constraint c
    ON c.conrelid = t.oid
INNER JOIN LATERAL (
    SELECT ROW_NUMBER() OVER(), * FROM UNNEST(c.conkey)
) p(col_pos, col_num)
    ON TRUE
INNER JOIN pg_catalog.pg_attribute a
    ON a.attrelid = t.oid
        AND a.attnum = p.col_num
        AND NOT a.attisdropped
WHERE t.relkind = 'r'
    AND c.contype = 'u'
    AND n.nspname NOT IN ('information_schema', 'pg_catalog')
    AND t.relname != '{DNDBTSysTables.DNDBTDbObjects}';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        internal class UniqueConstraintRecord : UniqueConstraintsBuilder.UniqueConstraintRecord { }
        internal static class ResultsInterpreter
        {
            public static void BuildTablesUniqueConstraints(
                Dictionary<string, Table> tables,
                IEnumerable<UniqueConstraintRecord> uniqueConstraintRecords)
            {
                UniqueConstraintsBuilder.BuildTablesUniqueConstraints(tables, uniqueConstraintRecords);
            }
        }
    }
}
