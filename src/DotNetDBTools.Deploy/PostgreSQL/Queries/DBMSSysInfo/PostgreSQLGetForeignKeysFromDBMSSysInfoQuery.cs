using System;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo;

internal class PostgreSQLGetForeignKeysFromDBMSSysInfoQuery : GetForeignKeysFromDBMSSysInfoQuery
{
    public override string Sql =>
$@"SELECT
    this_table.relname AS ""{nameof(ForeignKeyRecord.ThisTableName)}"",
    this_table_constraint.conname AS ""{nameof(ForeignKeyRecord.ForeignKeyName)}"",
    this_columns.attname AS ""{nameof(ForeignKeyRecord.ThisColumnName)}"",
    this_columns_positions.col_pos AS ""{nameof(ForeignKeyRecord.ThisColumnPosition)}"",
    referenced_table.relname AS ""{nameof(ForeignKeyRecord.ReferencedTableName)}"",
    referenced_columns.attname AS ""{nameof(ForeignKeyRecord.ReferencedColumnName)}"",
    referenced_columns_positions.col_pos AS ""{nameof(ForeignKeyRecord.ReferencedColumnPosition)}"",
    this_table_constraint.confupdtype AS ""{nameof(ForeignKeyRecord.OnUpdate)}"",
    this_table_constraint.confdeltype AS ""{nameof(ForeignKeyRecord.OnDelete)}""
FROM pg_catalog.pg_class this_table
INNER JOIN pg_catalog.pg_namespace this_table_ns
    ON this_table_ns.oid = this_table.relnamespace
INNER JOIN pg_catalog.pg_constraint this_table_constraint
    ON this_table_constraint.conrelid = this_table.oid
INNER JOIN pg_catalog.pg_class referenced_table
    ON referenced_table.oid = this_table_constraint.confrelid
INNER JOIN LATERAL (
    SELECT ROW_NUMBER() OVER(), * FROM UNNEST(this_table_constraint.conkey)
) this_columns_positions(col_pos, col_num)
    ON TRUE
INNER JOIN LATERAL (
    SELECT ROW_NUMBER() OVER(), * FROM UNNEST(this_table_constraint.confkey)
) referenced_columns_positions(col_pos, col_num)
    ON referenced_columns_positions.col_pos = this_columns_positions.col_pos
INNER JOIN pg_catalog.pg_attribute this_columns
    ON this_columns.attrelid = this_table.oid
        AND this_columns.attnum = this_columns_positions.col_num
        AND NOT this_columns.attisdropped
INNER JOIN pg_catalog.pg_attribute referenced_columns
    ON referenced_columns.attrelid = referenced_table.oid
        AND referenced_columns.attnum = referenced_columns_positions.col_num
        AND NOT referenced_columns.attisdropped
WHERE this_table.relkind = 'r'
    AND this_table_constraint.contype = 'f'
    AND this_table_ns.nspname NOT IN ('information_schema', 'pg_catalog')
    AND this_table.relname NOT IN ({DNDBTSysTables.AllTablesForInClause});";

    public override RecordMapper Mapper => new PostgreSQLRecordMapper();

    public class PostgreSQLRecordMapper : RecordMapper
    {
        public override ForeignKey MapExceptColumnsToForeignKeyModel(ForeignKeyRecord fkr)
        {
            return new ForeignKey()
            {
                ID = Guid.NewGuid(),
                Name = fkr.ForeignKeyName,
                ReferencedTableName = fkr.ReferencedTableName,
                OnUpdate = MapUpdateActionName(fkr.OnUpdate),
                OnDelete = MapUpdateActionName(fkr.OnDelete),
            };
        }

        private static string MapUpdateActionName(string sqlActionName)
        {
            return sqlActionName switch
            {
                "a" => ForeignKeyActions.NoAction,
                "r" => ForeignKeyActions.Restrict,
                "c" => ForeignKeyActions.Cascade,
                "d" => ForeignKeyActions.SetDefault,
                "n" => ForeignKeyActions.SetNull,
                _ => throw new InvalidOperationException($"Invalid sqlActionName: '{sqlActionName}'")
            };
        }
    }
}
