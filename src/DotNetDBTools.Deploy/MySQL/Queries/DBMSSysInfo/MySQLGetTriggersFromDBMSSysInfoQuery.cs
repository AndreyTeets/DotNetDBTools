using System;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MySQL.Queries.DBMSSysInfo
{
    internal class MySQLGetTriggersFromDBMSSysInfoQuery : GetTriggersFromDBMSSysInfoQuery
    {
        public override string Sql =>
$@"SELECT
    t.EVENT_OBJECT_TABLE AS {nameof(TriggerRecord.TableName)},
    t.TRIGGER_NAME AS {nameof(TriggerRecord.TriggerName)},
    CONCAT(
        'CREATE TRIGGER `', t.TRIGGER_NAME, '` ',
        ACTION_TIMING, ' ', EVENT_MANIPULATION, ' ON `', t.EVENT_OBJECT_TABLE,
        '` FOR EACH ROW ', t.ACTION_STATEMENT
    ) AS {nameof(TriggerRecord.TriggerCode)}
FROM INFORMATION_SCHEMA.TRIGGERS t
WHERE t.TRIGGER_SCHEMA = (select DATABASE());";

        public override RecordMapper Mapper => new MySQLRecordMapper();

        public class MySQLRecordMapper : RecordMapper
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
}
