using System;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DBMSSysInfo;

internal class MSSQLGetIndexesFromDBMSSysInfoQuery : GetIndexesFromDBMSSysInfoQuery
{
    public override string Sql =>
$@"SELECT
    t.name AS {nameof(IndexRecord.TableName)},
    i.name AS {nameof(IndexRecord.IndexName)},
    i.is_unique AS {nameof(IndexRecord.IsUnique)},
    col.name AS {nameof(IndexRecord.ColumnName)},
    ic.is_included_column AS {nameof(IndexRecord.IsIncludeColumn)},
    CASE WHEN ic.is_included_column = 1
        THEN ic.index_column_id
        ELSE ic.key_ordinal
    END AS {nameof(IndexRecord.ColumnPosition)}
FROM sys.indexes i
INNER JOIN sys.tables t
    ON t.object_id = i.object_id
INNER JOIN sys.index_columns ic
    ON ic.object_id = i.object_id
        AND ic.index_id = i.index_id
INNER JOIN sys.columns col
    ON col.object_id = ic.object_id
        AND col.column_id = ic.column_id
WHERE i.is_primary_key = 0
     AND i.is_unique_constraint = 0
     AND t.is_ms_shipped = 0;";

    public override RecordMapper Mapper => new MSSQLRecordMapper();

    public class MSSQLRecordMapper : RecordMapper
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
