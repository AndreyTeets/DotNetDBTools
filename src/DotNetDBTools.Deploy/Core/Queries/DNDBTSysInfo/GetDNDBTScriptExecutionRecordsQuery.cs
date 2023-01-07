using System;
using System.Collections.Generic;

namespace DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

internal abstract class GetDNDBTScriptExecutionRecordsQuery : NoParametersQuery
{
    public abstract RecordsLoader Loader { get; }

    public abstract class ScriptRecord
    {
        public abstract Guid GetID();
        public string Name { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public long MinDbVersionToExecute { get; set; }
        public long MaxDbVersionToExecute { get; set; }
    }

    public abstract class RecordsLoader
    {
        public abstract IEnumerable<ScriptRecord> GetRecords(IQueryExecutor queryExecutor, GetDNDBTScriptExecutionRecordsQuery query);
    }
}
