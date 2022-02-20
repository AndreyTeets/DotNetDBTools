using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;

internal class SQLiteGetDNDBTScriptExecutionRecordsQuery : GetDNDBTScriptExecutionRecordsQuery
{
    public override string Sql =>
$@"SELECT
    [{DNDBTSysTables.DNDBTScriptExecutions.ID}] AS [{nameof(SQLiteScriptRecord.ID)}],
    [{DNDBTSysTables.DNDBTScriptExecutions.Type}] AS [{nameof(SQLiteScriptRecord.Type)}],
    [{DNDBTSysTables.DNDBTScriptExecutions.Name}] AS [{nameof(SQLiteScriptRecord.Name)}],
    [{DNDBTSysTables.DNDBTScriptExecutions.Code}] AS [{nameof(SQLiteScriptRecord.Code)}],
    [{DNDBTSysTables.DNDBTScriptExecutions.MinDbVersionToExecute}] AS [{nameof(SQLiteScriptRecord.MinDbVersionToExecute)}],
    [{DNDBTSysTables.DNDBTScriptExecutions.MaxDbVersionToExecute}] AS [{nameof(SQLiteScriptRecord.MaxDbVersionToExecute)}]
FROM [{DNDBTSysTables.DNDBTScriptExecutions}];";

    public override RecordsLoader Loader => new SQLiteRecordsLoader();

    public class SQLiteScriptRecord : ScriptRecord
    {
        public string ID { get; set; }
        public override Guid GetID() => new(ID);
    }

    public class SQLiteRecordsLoader : RecordsLoader
    {
        public override IEnumerable<ScriptRecord> GetRecords(IQueryExecutor queryExecutor, GetDNDBTScriptExecutionRecordsQuery query)
        {
            return queryExecutor.Query<SQLiteScriptRecord>(query);
        }
    }
}
