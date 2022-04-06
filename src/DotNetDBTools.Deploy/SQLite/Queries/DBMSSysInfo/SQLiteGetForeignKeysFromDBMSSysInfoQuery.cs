using System;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.SQLite.Queries.DBMSSysInfo;

internal class SQLiteGetForeignKeysFromDBMSSysInfoQuery : GetForeignKeysFromDBMSSysInfoQuery
{
    public override string Sql =>
$@"SELECT
    sm.name AS [{nameof(ForeignKeyRecord.ThisTableName)}],
    'FK_' || sm.name || '_' || fkl.[table] || '_' || fkl.id AS [{nameof(ForeignKeyRecord.ForeignKeyName)}],
    fkl.[from] AS [{nameof(ForeignKeyRecord.ThisColumnName)}],
    fkl.seq AS [{nameof(ForeignKeyRecord.ThisColumnPosition)}],
    fkl.[table] AS [{nameof(ForeignKeyRecord.ReferencedTableName)}],
    fkl.[to] AS [{nameof(ForeignKeyRecord.ReferencedColumnName)}],
    fkl.seq AS [{nameof(ForeignKeyRecord.ReferencedColumnPosition)}],
    fkl.on_update AS [{nameof(ForeignKeyRecord.OnUpdate)}],
    fkl.on_delete AS [{nameof(ForeignKeyRecord.OnDelete)}]
FROM sqlite_master sm
INNER JOIN pragma_foreign_key_list(sm.name) fkl
WHERE sm.type = 'table'
    AND sm.name != 'sqlite_sequence'
    AND sm.name NOT IN ({DNDBTSysTables.AllTablesForInClause});";

    public override RecordMapper Mapper => new MySQLRecordMapper();

    public class MySQLRecordMapper : RecordMapper
    {
        public override ForeignKey MapExceptColumnsToForeignKeyModel(ForeignKeyRecord fkr)
        {
            return new ForeignKey()
            {
                ID = Guid.NewGuid(),
                Name = fkr.ForeignKeyName,
                ReferencedTableName = fkr.ReferencedTableName,
                OnUpdate = fkr.OnUpdate,
                OnDelete = fkr.OnDelete,
            };
        }
    }
}
