using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo;

internal class PostgreSQLGetColumnsFromDBMSSysInfoQuery : GetColumnsFromDBMSSysInfoQuery
{
    public override string Sql =>
$@"SELECT
    c.relname AS ""{nameof(PostgreSQLColumnRecord.TableName)}"",
    a.attname AS ""{nameof(PostgreSQLColumnRecord.ColumnName)}"",
    t.typname AS ""{nameof(PostgreSQLColumnRecord.DataType)}"",
    a.attnotnull AS ""{nameof(PostgreSQLColumnRecord.NotNull)}"",
    a.attidentity AS ""{nameof(PostgreSQLColumnRecord.Identity)}"",
    pg_catalog.pg_get_expr(d.adbin, d.adrelid) AS ""{nameof(PostgreSQLColumnRecord.Default)}"",
    a.atttypmod AS ""{nameof(PostgreSQLColumnRecord.Length)}"",
    a.attndims AS ""{nameof(PostgreSQLColumnRecord.ArrayDims)}"",
    et.typname AS ""{nameof(PostgreSQLColumnRecord.ArrayElemDataType)}"",
    s.seqstart AS ""{nameof(PostgreSQLColumnRecord.IdentitySequenceStartWith)}"",
    s.seqincrement AS ""{nameof(PostgreSQLColumnRecord.IdentitySequenceIncrementBy)}"",
    s.seqmin AS ""{nameof(PostgreSQLColumnRecord.IdentitySequenceMinValue)}"",
    s.seqmax AS ""{nameof(PostgreSQLColumnRecord.IdentitySequenceMaxValue)}"",
    s.seqcache AS ""{nameof(PostgreSQLColumnRecord.IdentitySequenceCache)}"",
    s.seqcycle AS ""{nameof(PostgreSQLColumnRecord.IdentitySequenceCycle)}""
FROM pg_catalog.pg_class c
INNER JOIN pg_catalog.pg_namespace n
    ON n.oid = c.relnamespace
INNER JOIN pg_catalog.pg_attribute a
    ON a.attrelid = c.oid
        AND a.attnum > 0
        AND NOT a.attisdropped
INNER JOIN pg_catalog.pg_type t
    ON t.oid = a.atttypid
LEFT JOIN pg_catalog.pg_type et
    ON et.oid = t.typelem
LEFT JOIN pg_catalog.pg_attrdef d
    ON (d.adrelid, d.adnum) = (a.attrelid, a.attnum)
LEFT JOIN LATERAL (
    SELECT sq.*
    FROM pg_catalog.pg_sequence sq
    INNER JOIN pg_catalog.pg_depend dep
        ON dep.objid = sq.seqrelid
            AND dep.classid = 'pg_class'::regclass
            AND dep.refclassid = 'pg_class'::regclass
            AND dep.deptype = 'i'
            AND dep.refobjid = a.attrelid
            AND dep.refobjsubid = a.attnum
    ) s ON (a.attidentity = 'a' OR a.attidentity = 'd')
WHERE c.relkind = 'r'
    AND n.nspname NOT IN ('information_schema', 'pg_catalog')
    AND c.relname NOT IN ({DNDBTSysTables.AllTablesForInClause});";

    public override RecordsLoader Loader => new PostgreSQLRecordsLoader();
    public override RecordMapper Mapper => new PostgreSQLRecordMapper();

    public class PostgreSQLColumnRecord : ColumnRecord
    {
        public string Identity { get; set; }
        public string Length { get; set; }
        public int ArrayDims { get; set; }
        public string ArrayElemDataType { get; set; }
        public long IdentitySequenceStartWith { get; set; }
        public long IdentitySequenceIncrementBy { get; set; }
        public long IdentitySequenceMinValue { get; set; }
        public long IdentitySequenceMaxValue { get; set; }
        public long IdentitySequenceCache { get; set; }
        public bool IdentitySequenceCycle { get; set; }
    }

    public class PostgreSQLRecordsLoader : RecordsLoader
    {
        public override IEnumerable<ColumnRecord> GetRecords(IQueryExecutor queryExecutor, GetColumnsFromDBMSSysInfoQuery query)
        {
            return queryExecutor.Query<PostgreSQLColumnRecord>(query);
        }
    }

    public class PostgreSQLRecordMapper : RecordMapper
    {
        public override Column MapToColumnModel(ColumnRecord columnRecord)
        {
            PostgreSQLColumnRecord cr = (PostgreSQLColumnRecord)columnRecord;
            DataType dataType = PostgreSQLQueriesHelper.CreateDataTypeModel(
                cr.ArrayDims > 0 ? cr.ArrayElemDataType : cr.DataType,
                cr.Length,
                cr.ArrayDims);

            PostgreSQLColumn columnModel = new PostgreSQLColumn()
            {
                ID = Guid.NewGuid(),
                Name = cr.ColumnName,
                DataType = dataType,
                NotNull = cr.NotNull,
                Identity = cr.Identity == "a" || cr.Identity == "d",
                Default = PostgreSQLQueriesHelper.ParseDefault(cr.Default),
            };

            if (columnModel.Identity)
            {
                columnModel.IdentityGenerationKind = cr.Identity == "a" ? "ALWAYS" : "BY DEFAULT";
                columnModel.IdentitySequenceOptions = new PostgreSQLSequenceOptions()
                {
                    StartWith = cr.IdentitySequenceStartWith,
                    IncrementBy = cr.IdentitySequenceIncrementBy,
                    MinValue = cr.IdentitySequenceMinValue,
                    MaxValue = cr.IdentitySequenceMaxValue,
                    Cache = cr.IdentitySequenceCache,
                    Cycle = cr.IdentitySequenceCycle,
                };
            }

            return columnModel;
        }
    }
}
