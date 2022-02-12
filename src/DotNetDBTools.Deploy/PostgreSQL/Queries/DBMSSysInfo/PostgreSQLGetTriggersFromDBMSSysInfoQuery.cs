using System;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo;

internal class PostgreSQLGetTriggersFromDBMSSysInfoQuery : GetTriggersFromDBMSSysInfoQuery
{
    public override string Sql =>
$@"SELECT
    tb.relname AS ""{nameof(TriggerRecord.TableName)}"",
    tr.tgname AS ""{nameof(TriggerRecord.TriggerName)}"",
    pg_catalog.pg_get_triggerdef(tr.oid) AS ""{nameof(TriggerRecord.TriggerCode)}""
FROM pg_catalog.pg_trigger tr
INNER JOIN pg_catalog.pg_class tb
    ON tb.oid = tr.tgrelid
INNER JOIN pg_catalog.pg_namespace n
    ON n.oid = tb.relnamespace
WHERE tr.tgisinternal = FALSE
    AND n.nspname NOT IN ('information_schema', 'pg_catalog');";

    public override RecordMapper Mapper => new PostgreSQLRecordMapper();

    public class PostgreSQLRecordMapper : RecordMapper
    {
        public override Trigger MapToTriggerModel(TriggerRecord triggerRecord)
        {
            return new()
            {
                ID = Guid.NewGuid(),
                Name = triggerRecord.TriggerName,
                CodePiece = new CodePiece { Code = triggerRecord.TriggerCode },
            };
        }
    }
}
