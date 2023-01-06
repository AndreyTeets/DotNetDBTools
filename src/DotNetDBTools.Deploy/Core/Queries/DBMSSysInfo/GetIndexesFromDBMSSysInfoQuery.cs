using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;

internal abstract class GetIndexesFromDBMSSysInfoQuery : NoParametersQuery
{
    public abstract RecordMapper Mapper { get; }

    public class IndexRecord
    {
        public string TableName { get; set; }
        public string IndexName { get; set; }
        public bool IsUnique { get; set; }
        public string ColumnName { get; set; }
        public bool IsIncludeColumn { get; set; }
        public int ColumnPosition { get; set; }
    }

    public abstract class RecordMapper
    {
        public abstract Index MapExceptColumnsToIndexModel(IndexRecord indexRecord);
    }
}
