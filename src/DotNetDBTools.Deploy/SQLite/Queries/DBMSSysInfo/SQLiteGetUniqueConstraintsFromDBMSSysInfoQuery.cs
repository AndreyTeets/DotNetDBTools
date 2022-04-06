using System;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.SQLite.Queries.DBMSSysInfo;

internal class SQLiteGetUniqueConstraintsFromDBMSSysInfoQuery : GetUniqueConstraintsFromDBMSSysInfoQuery
{
    public override string Sql =>
$@"SELECT
    sm.name AS [{nameof(UniqueConstraintRecord.TableName)}],
    'UQ_' || sm.name || '_' || il.seq AS [{nameof(UniqueConstraintRecord.ConstraintName)}],
    ii.name AS [{nameof(UniqueConstraintRecord.ColumnName)}],
    ii.seqno AS [{nameof(UniqueConstraintRecord.ColumnPosition)}]
FROM sqlite_master sm
INNER JOIN pragma_index_list(sm.name) il
INNER JOIN pragma_index_info(il.name) ii
WHERE sm.type = 'table'
    AND sm.name != 'sqlite_sequence'
    AND sm.name NOT IN ({DNDBTSysTables.AllTablesForInClause})
    AND il.origin = 'u';";

    public override RecordMapper Mapper => new SQLiteRecordMapper();

    public class SQLiteRecordMapper : RecordMapper
    {
        public override UniqueConstraint MapExceptColumnsToUniqueConstraintModel(UniqueConstraintRecord ucr)
        {
            return new UniqueConstraint()
            {
                ID = Guid.NewGuid(),
                Name = ucr.ConstraintName,
            };
        }
    }
}
