using System;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DBMSSysInfo;

internal class MSSQLGetForeignKeysFromDBMSSysInfoQuery : GetForeignKeysFromDBMSSysInfoQuery
{
    public override string Sql =>
$@"SELECT
    thisTable.name AS [{nameof(ForeignKeyRecord.ThisTableName)}],
    foreignKey.name AS [{nameof(ForeignKeyRecord.ForeignKeyName)}],
    thisColumns.name AS [{nameof(ForeignKeyRecord.ThisColumnName)}],
    fkColumnsMap.constraint_column_id AS [{nameof(ForeignKeyRecord.ThisColumnPosition)}],
    referencedTable.name AS [{nameof(ForeignKeyRecord.ReferencedTableName)}],
    referencedColumns.name AS [{nameof(ForeignKeyRecord.ReferencedColumnName)}],
    fkColumnsMap.constraint_column_id AS [{nameof(ForeignKeyRecord.ReferencedColumnPosition)}],
    foreignKey.update_referential_action_desc AS [{nameof(ForeignKeyRecord.OnUpdate)}],
    foreignKey.delete_referential_action_desc AS [{nameof(ForeignKeyRecord.OnDelete)}]
FROM sys.tables thisTable
INNER JOIN sys.foreign_keys foreignKey
    ON foreignKey.parent_object_id = thisTable.object_id
INNER JOIN sys.tables referencedTable
    ON referencedTable.object_id = foreignKey.referenced_object_id
INNER JOIN sys.foreign_key_columns fkColumnsMap
    ON fkColumnsMap.constraint_object_id = foreignKey.object_id
INNER JOIN sys.columns thisColumns
    ON thisColumns.object_id = foreignKey.parent_object_id
        AND thisColumns.column_id = fkColumnsMap.parent_column_id
INNER JOIN sys.columns referencedColumns
    ON referencedColumns.object_id = foreignKey.referenced_object_id
        AND referencedColumns.column_id = fkColumnsMap.referenced_column_id
WHERE thisTable.name NOT IN ({DNDBTSysTables.AllTablesForInClause});";

    public override RecordMapper Mapper => new MSSQLRecordMapper();

    public class MSSQLRecordMapper : RecordMapper
    {
        public override ForeignKey MapExceptColumnsToForeignKeyModel(ForeignKeyRecord fkr)
        {
            return new ForeignKey()
            {
                ID = Guid.NewGuid(),
                Name = fkr.ForeignKeyName,
                ThisTableName = fkr.ThisTableName,
                ReferencedTableName = fkr.ReferencedTableName,
                OnUpdate = MapUpdateActionName(fkr.OnUpdate),
                OnDelete = MapUpdateActionName(fkr.OnDelete),
            };
        }

        private static string MapUpdateActionName(string sqlActionName)
        {
            return sqlActionName switch
            {
                "NO_ACTION" => ForeignKeyActions.NoAction,
                "CASCADE" => ForeignKeyActions.Cascade,
                "SET_DEFAULT" => ForeignKeyActions.SetDefault,
                "SET_NULL" => ForeignKeyActions.SetNull,
                _ => throw new InvalidOperationException($"Invalid sqlActionName: '{sqlActionName}'")
            };
        }
    }
}
