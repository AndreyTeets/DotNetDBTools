using System;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MySQL.Queries.DBMSSysInfo;

internal class MySQLGetIndexesFromDBMSSysInfoQuery : GetIndexesFromDBMSSysInfoQuery
{
    public override string Sql =>
$@"SELECT
    s.TABLE_NAME AS {nameof(IndexRecord.TableName)},
    s.INDEX_NAME AS {nameof(IndexRecord.IndexName)},
    NOT s.NON_UNIQUE AS {nameof(IndexRecord.IsUnique)},
    s.COLUMN_NAME AS {nameof(IndexRecord.ColumnName)},
    FALSE AS {nameof(IndexRecord.IsIncludeColumn)},
    s.SEQ_IN_INDEX AS {nameof(IndexRecord.ColumnPosition)}
FROM INFORMATION_SCHEMA.STATISTICS s
LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
    ON tc.CONSTRAINT_SCHEMA = s.TABLE_SCHEMA
        AND tc.TABLE_NAME = s.TABLE_NAME
        AND tc.CONSTRAINT_NAME = s.INDEX_NAME
WHERE s.TABLE_SCHEMA = (select DATABASE())
    AND (tc.CONSTRAINT_TYPE = 'UNIQUE' OR tc.CONSTRAINT_TYPE IS NULL)
    AND s.TABLE_NAME != '{DNDBTSysTables.DNDBTDbObjects}';";

    public override RecordMapper Mapper => new MySQLRecordMapper();

    public class MySQLRecordMapper : RecordMapper
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
