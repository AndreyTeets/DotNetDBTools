using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo;

internal class MySQLGetDNDBTScriptExecutionRecordsQuery : GetDNDBTScriptExecutionRecordsQuery
{
    public override string Sql =>
$@"SELECT
    CAST(`{DNDBTSysTables.DNDBTScriptExecutions.ID}` AS CHAR(36)) AS `{nameof(MySQLScriptRecord.ID)}`,
    `{DNDBTSysTables.DNDBTScriptExecutions.Type}` AS `{nameof(MySQLScriptRecord.Type)}`,
    `{DNDBTSysTables.DNDBTScriptExecutions.Name}` AS `{nameof(MySQLScriptRecord.Name)}`,
    `{DNDBTSysTables.DNDBTScriptExecutions.Code}` AS `{nameof(MySQLScriptRecord.Code)}`,
    `{DNDBTSysTables.DNDBTScriptExecutions.MinDbVersionToExecute}` AS `{nameof(MySQLScriptRecord.MinDbVersionToExecute)}`,
    `{DNDBTSysTables.DNDBTScriptExecutions.MaxDbVersionToExecute}` AS `{nameof(MySQLScriptRecord.MaxDbVersionToExecute)}`
FROM `{DNDBTSysTables.DNDBTScriptExecutions}`;";

    public override RecordsLoader Loader => new MySQLRecordsLoader();

    public class MySQLScriptRecord : ScriptRecord
    {
        public string ID { get; set; }
        public override Guid GetID() => new(ID);
    }

    public class MySQLRecordsLoader : RecordsLoader
    {
        public override IEnumerable<ScriptRecord> GetRecords(IQueryExecutor queryExecutor, GetDNDBTScriptExecutionRecordsQuery query)
        {
            return queryExecutor.Query<MySQLScriptRecord>(query);
        }
    }
}
