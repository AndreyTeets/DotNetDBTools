using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo;

internal class MSSQLGetDNDBTScriptExecutionRecordsQuery : GetDNDBTScriptExecutionRecordsQuery
{
    public override string Sql =>
$@"SELECT
    [{DNDBTSysTables.DNDBTScriptExecutions.ID}] AS [{nameof(MSSQLScriptRecord.ID)}],
    [{DNDBTSysTables.DNDBTScriptExecutions.Type}] AS [{nameof(MSSQLScriptRecord.Type)}],
    [{DNDBTSysTables.DNDBTScriptExecutions.Name}] AS [{nameof(MSSQLScriptRecord.Name)}],
    [{DNDBTSysTables.DNDBTScriptExecutions.Text}] AS [{nameof(MSSQLScriptRecord.Text)}],
    [{DNDBTSysTables.DNDBTScriptExecutions.MinDbVersionToExecute}] AS [{nameof(MSSQLScriptRecord.MinDbVersionToExecute)}],
    [{DNDBTSysTables.DNDBTScriptExecutions.MaxDbVersionToExecute}] AS [{nameof(MSSQLScriptRecord.MaxDbVersionToExecute)}]
FROM [{DNDBTSysTables.DNDBTScriptExecutions}];";

    public override RecordsLoader Loader => new MSSQLRecordsLoader();

    public class MSSQLScriptRecord : ScriptRecord
    {
        public Guid ID { get; set; }
        public override Guid GetID() => ID;
    }

    public class MSSQLRecordsLoader : RecordsLoader
    {
        public override IEnumerable<ScriptRecord> GetRecords(IQueryExecutor queryExecutor, GetDNDBTScriptExecutionRecordsQuery query)
        {
            return queryExecutor.Query<MSSQLScriptRecord>(query);
        }
    }
}
