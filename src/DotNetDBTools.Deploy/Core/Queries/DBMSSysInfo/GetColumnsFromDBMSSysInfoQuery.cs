using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;

internal abstract class GetColumnsFromDBMSSysInfoQuery : NoParametersQuery
{
    public abstract RecordsLoader Loader { get; }
    public abstract RecordMapper Mapper { get; }

    public abstract class ColumnRecord
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public bool NotNull { get; set; }
        public string Default { get; set; }
    }

    public abstract class RecordsLoader
    {
        public abstract IEnumerable<ColumnRecord> GetRecords(IQueryExecutor queryExecutor, GetColumnsFromDBMSSysInfoQuery query);
    }

    public abstract class RecordMapper
    {
        public abstract Column MapToColumnModel(ColumnRecord columnRecord);
    }
}
