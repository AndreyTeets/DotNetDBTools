using DotNetDBTools.Deploy.Core.Queries;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo;

internal class PostgreSQLGetSequencesFromDBMSSysInfoQuery : NoParametersQuery
{
    public override string Sql =>
$@"SELECT
    c.relname AS ""{nameof(SequenceRecord.SequenceName)}"",
    t.typname AS ""{nameof(SequenceRecord.DataType)}"",
    s.seqstart AS ""{nameof(SequenceRecord.StartWith)}"",
    s.seqincrement AS ""{nameof(SequenceRecord.IncrementBy)}"",
    s.seqmin AS ""{nameof(SequenceRecord.MinValue)}"",
    s.seqmax AS ""{nameof(SequenceRecord.MaxValue)}"",
    s.seqcycle AS ""{nameof(SequenceRecord.Cycle)}"",
    s.seqcache AS ""{nameof(SequenceRecord.Cache)}"",
    depc.relname AS ""{nameof(SequenceRecord.OwnedByTableName)}"",
    depa.attname AS ""{nameof(SequenceRecord.OwnedByColumnName)}""
FROM pg_catalog.pg_sequence s
INNER JOIN pg_catalog.pg_class c
    ON c.oid = s.seqrelid
INNER JOIN pg_catalog.pg_namespace n
    ON n.oid = c.relnamespace
INNER JOIN pg_catalog.pg_type t
    ON t.oid = s.seqtypid
LEFT JOIN pg_catalog.pg_depend dep
    ON dep.objid = c.oid
        AND dep.classid = 'pg_class'::regclass
        AND dep.refclassid = 'pg_class'::regclass
LEFT JOIN pg_catalog.pg_class depc
    ON depc.oid = dep.refobjid
LEFT JOIN pg_catalog.pg_attribute depa
    ON depa.attrelid = depc.oid
        AND depa.attnum = dep.refobjsubid
WHERE n.nspname NOT IN ('information_schema', 'pg_catalog')
    AND a.attidentity = '';";

    public class SequenceRecord
    {
        public string SequenceName { get; set; }
        public string DataType { get; set; }
        public long StartWith { get; set; }
        public long IncrementBy { get; set; }
        public long MinValue { get; set; }
        public long MaxValue { get; set; }
        public bool Cycle { get; set; }
        public long Cache { get; set; }
        public string OwnedByTableName { get; set; }
        public string OwnedByColumnName { get; set; }
    }
}
