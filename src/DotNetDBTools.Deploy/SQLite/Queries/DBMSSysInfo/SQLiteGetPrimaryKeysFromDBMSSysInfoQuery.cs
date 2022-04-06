using System;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.SQLite.Queries.DBMSSysInfo;

internal class SQLiteGetPrimaryKeysFromDBMSSysInfoQuery : GetPrimaryKeysFromDBMSSysInfoQuery
{
    public override string Sql =>
$@"SELECT
    sm.name AS [{nameof(PrimaryKeyRecord.TableName)}],
    'PK_' || sm.name AS [{nameof(PrimaryKeyRecord.ConstraintName)}],
    ii.name AS [{nameof(PrimaryKeyRecord.ColumnName)}],
    ii.seqno AS [{nameof(PrimaryKeyRecord.ColumnPosition)}]
FROM sqlite_master sm
INNER JOIN pragma_index_list(sm.name) il
INNER JOIN pragma_index_info(il.name) ii
WHERE sm.type = 'table'
    AND sm.name != 'sqlite_sequence'
    AND sm.name NOT IN ({DNDBTSysTables.AllTablesForInClause})
    AND il.origin = 'pk'
UNION
SELECT
    sm.name AS [{nameof(PrimaryKeyRecord.TableName)}],
    'PK_' || sm.name AS [{nameof(PrimaryKeyRecord.ConstraintName)}],
    ti.name AS [{nameof(PrimaryKeyRecord.ColumnName)}],
    0 AS [{nameof(PrimaryKeyRecord.ColumnPosition)}]
FROM sqlite_master sm
INNER JOIN pragma_table_info(sm.name) ti
WHERE sm.type = 'table'
    AND sm.name != 'sqlite_sequence'
    AND sm.name NOT IN ({DNDBTSysTables.AllTablesForInClause})
    AND ti.pk = 1
    AND lower(ti.type) = 'integer';";

    public override RecordMapper Mapper => new SQLiteRecordMapper();

    public class SQLiteRecordMapper : RecordMapper
    {
        public override PrimaryKey MapExceptColumnsToPrimaryKeyModel(PrimaryKeyRecord pkr)
        {
            return new PrimaryKey()
            {
                ID = Guid.NewGuid(),
                Name = pkr.ConstraintName,
            };
        }
    }
}
