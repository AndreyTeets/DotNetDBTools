﻿using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.ModelBuilders;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo
{
    internal class GetPrimaryKeysFromDBMSSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    t.relname AS ""{nameof(PrimaryKeyRecord.TableName)}"",
    c.conname AS ""{nameof(PrimaryKeyRecord.ConstraintName)}"",
    a.attname AS ""{nameof(PrimaryKeyRecord.ColumnName)}"",
    p.col_pos AS ""{nameof(PrimaryKeyRecord.ColumnPosition)}""
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
    AND c.contype = 'p'
    AND n.nspname NOT IN ('information_schema', 'pg_catalog')
    AND t.relname != '{DNDBTSysTables.DNDBTDbObjects}';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        internal class PrimaryKeyRecord : PrimaryKeysBuilder.PrimaryKeyRecord { }
        internal static class ResultsInterpreter
        {
            public static void BuildTablesPrimaryKeys(
                Dictionary<string, Table> tables,
                IEnumerable<PrimaryKeyRecord> primaryKeyRecords)
            {
                PrimaryKeysBuilder.BuildTablesPrimaryKeys(tables, primaryKeyRecords);
            }
        }
    }
}
