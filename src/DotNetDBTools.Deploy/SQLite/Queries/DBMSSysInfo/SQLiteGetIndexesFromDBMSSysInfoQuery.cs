using System;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.SQLite.Queries.DBMSSysInfo
{
    internal class SQLiteGetIndexesFromDBMSSysInfoQuery : GetIndexesFromDBMSSysInfoQuery
    {
        public override string Sql =>
$@"SELECT
    sm.name AS {nameof(IndexRecord.TableName)},
    il.name AS {nameof(IndexRecord.IndexName)},
    il.[unique] AS {nameof(IndexRecord.IsUnique)},
    ii.name AS {nameof(IndexRecord.ColumnName)},
    FALSE AS {nameof(IndexRecord.IsIncludeColumn)},
    ii.seqno AS {nameof(IndexRecord.ColumnPosition)}
FROM sqlite_master sm
INNER JOIN pragma_index_list(sm.name) il
INNER JOIN pragma_index_info(il.name) ii
WHERE sm.type = 'table'
    AND sm.name != 'sqlite_sequence'
    AND sm.name != '{DNDBTSysTables.DNDBTDbObjects}'
    AND il.origin = 'c';";

        public override RecordMapper Mapper => new SQLiteRecordMapper();

        public class SQLiteRecordMapper : RecordMapper
        {
            public override Index MapExceptColumnsToIndexModel(IndexRecord indexRecord)
            {
                return new()
                {
                    ID = Guid.NewGuid(),
                    Name = indexRecord.IndexName,
                    Unique = indexRecord.IsUnique,
                };
            }
        }
    }
}
