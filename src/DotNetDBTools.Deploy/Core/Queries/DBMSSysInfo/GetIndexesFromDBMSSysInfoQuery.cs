using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;

internal abstract class GetIndexesFromDBMSSysInfoQuery : NoParametersQuery
{
    public virtual RecordsLoader Loader => new RecordsLoader();
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

    public class RecordsLoader
    {
        public virtual IEnumerable<IndexRecord> GetRecords(IQueryExecutor queryExecutor, GetIndexesFromDBMSSysInfoQuery query)
        {
            return queryExecutor.Query<IndexRecord>(query);
        }
    }

    public abstract class RecordMapper
    {
        public abstract Index MapExceptColumnsToIndexModel(IndexRecord indexRecord);
    }
}
