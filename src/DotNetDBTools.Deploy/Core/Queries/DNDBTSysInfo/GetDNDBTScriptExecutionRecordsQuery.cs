using System;
using System.Collections.Generic;

namespace DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

internal abstract class GetDNDBTScriptExecutionRecordsQuery : IQuery
{
    public abstract string Sql { get; }
    public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    public abstract RecordsLoader Loader { get; }

    public abstract class ScriptRecord
    {
        public abstract Guid GetID();
        public string Name { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public long MinDbVersionToExecute { get; set; }
        public long MaxDbVersionToExecute { get; set; }
    }

    public abstract class RecordsLoader
    {
        public abstract IEnumerable<ScriptRecord> GetRecords(IQueryExecutor queryExecutor, GetDNDBTScriptExecutionRecordsQuery query);
    }
}
