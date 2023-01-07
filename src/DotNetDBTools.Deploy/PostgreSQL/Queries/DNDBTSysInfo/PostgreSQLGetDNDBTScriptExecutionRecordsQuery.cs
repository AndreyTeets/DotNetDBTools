using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;

internal class PostgreSQLGetDNDBTScriptExecutionRecordsQuery : GetDNDBTScriptExecutionRecordsQuery
{
    public override string Sql =>
$@"SELECT
    ""{DNDBTSysTables.DNDBTScriptExecutions.ID}"" AS ""{nameof(PostgreSQLScriptRecord.ID)}"",
    ""{DNDBTSysTables.DNDBTScriptExecutions.Type}"" AS ""{nameof(PostgreSQLScriptRecord.Type)}"",
    ""{DNDBTSysTables.DNDBTScriptExecutions.Name}"" AS ""{nameof(PostgreSQLScriptRecord.Name)}"",
    ""{DNDBTSysTables.DNDBTScriptExecutions.Text}"" AS ""{nameof(PostgreSQLScriptRecord.Text)}"",
    ""{DNDBTSysTables.DNDBTScriptExecutions.MinDbVersionToExecute}"" AS ""{nameof(PostgreSQLScriptRecord.MinDbVersionToExecute)}"",
    ""{DNDBTSysTables.DNDBTScriptExecutions.MaxDbVersionToExecute}"" AS ""{nameof(PostgreSQLScriptRecord.MaxDbVersionToExecute)}""
FROM ""{DNDBTSysTables.DNDBTScriptExecutions}"";";

    public override RecordsLoader Loader => new PostgreSQLRecordsLoader();

    public class PostgreSQLScriptRecord : ScriptRecord
    {
        public Guid ID { get; set; }
        public override Guid GetID() => ID;
    }

    public class PostgreSQLRecordsLoader : RecordsLoader
    {
        public override IEnumerable<ScriptRecord> GetRecords(IQueryExecutor queryExecutor, GetDNDBTScriptExecutionRecordsQuery query)
        {
            return queryExecutor.Query<PostgreSQLScriptRecord>(query);
        }
    }
}
