using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo;

internal class PostgreSQLGetColumnsFromDBMSSysInfoQuery : GetColumnsFromDBMSSysInfoQuery
{
    public override string Sql =>
$@"SELECT
    c.relname AS ""{nameof(PostgreSQLColumnRecord.TableName)}"",
    a.attname AS ""{nameof(PostgreSQLColumnRecord.ColumnName)}"",
    t.typname AS ""{nameof(PostgreSQLColumnRecord.DataType)}"",
    t.typtype = 'b' AS ""{nameof(PostgreSQLColumnRecord.IsBaseDataType)}"",
    a.attnotnull AS ""{nameof(PostgreSQLColumnRecord.NotNull)}"",
    pg_catalog.pg_get_serial_sequence('""' || n.nspname || '"".""' || c.relname || '""', a.attname) IS NOT NULL AS ""{nameof(PostgreSQLColumnRecord.Identity)}"",
    pg_catalog.pg_get_expr(d.adbin, d.adrelid) AS ""{nameof(PostgreSQLColumnRecord.Default)}"",
    a.atttypmod AS ""{nameof(PostgreSQLColumnRecord.Length)}""
FROM pg_catalog.pg_class c
INNER JOIN pg_catalog.pg_namespace n
    ON n.oid = c.relnamespace
INNER JOIN pg_catalog.pg_attribute a
    ON a.attrelid = c.oid
        AND a.attnum > 0
        AND NOT a.attisdropped
INNER JOIN pg_catalog.pg_type t
    ON t.oid = a.atttypid
LEFT JOIN pg_catalog.pg_attrdef d
    ON (d.adrelid, d.adnum) = (a.attrelid, a.attnum)
WHERE c.relkind = 'r'
    AND n.nspname NOT IN ('information_schema', 'pg_catalog')
    AND c.relname NOT IN ({DNDBTSysTables.AllTablesForInClause});";

    public override RecordsLoader Loader => new PostgreSQLRecordsLoader();
    public override RecordMapper Mapper => new PostgreSQLRecordMapper();

    public class PostgreSQLColumnRecord : ColumnRecord
    {
        public bool IsBaseDataType { get; set; }
        public bool Identity { get; set; }
        public string Length { get; set; }
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
                cr.DataType,
                cr.Length,
                cr.IsBaseDataType);
            return new Column()
            {
                ID = Guid.NewGuid(),
                Name = cr.ColumnName,
                DataType = dataType,
                NotNull = cr.NotNull,
                Identity = cr.Identity,
                Default = PostgreSQLQueriesHelper.ParseDefault(dataType, cr.Default),
            };
        }
    }
}
