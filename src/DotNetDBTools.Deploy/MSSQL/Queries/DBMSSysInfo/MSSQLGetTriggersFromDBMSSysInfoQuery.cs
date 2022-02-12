using System;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DBMSSysInfo;

internal class MSSQLGetTriggersFromDBMSSysInfoQuery : GetTriggersFromDBMSSysInfoQuery
{
    public override string Sql =>
$@"SELECT
    o.name AS {nameof(TriggerRecord.TableName)},
    t.name AS {nameof(TriggerRecord.TriggerName)},
    object_definition(t.object_id) AS {nameof(TriggerRecord.TriggerCode)}
FROM sys.triggers t
INNER JOIN sys.objects o
    ON t.parent_id = o.object_id
WHERE t.type = 'TR'
    AND t.parent_class = 1
    AND t.is_ms_shipped = 0;";

    public override RecordMapper Mapper => new MSSQLRecordMapper();

    public class MSSQLRecordMapper : RecordMapper
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
