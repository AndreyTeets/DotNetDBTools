using System;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Deploy.SQLite.Queries.DBMSSysInfo;

internal class SQLiteGetTriggersFromDBMSSysInfoQuery : GetTriggersFromDBMSSysInfoQuery
{
    public override string Sql =>
$@"SELECT
    sm.tbl_name AS [{nameof(TriggerRecord.TableName)}],
    sm.name AS [{nameof(TriggerRecord.TriggerName)}],
    sm.sql AS [{nameof(TriggerRecord.TriggerCode)}]
FROM sqlite_master sm
WHERE sm.type = 'trigger';";

    public override RecordMapper Mapper => new SQLiteRecordMapper();

    public class SQLiteRecordMapper : RecordMapper
    {
        public override Trigger MapToTriggerModel(TriggerRecord triggerRecord)
        {
            return new SQLiteTrigger()
            {
                ID = Guid.NewGuid(),
                Name = triggerRecord.TriggerName,
                TableName = triggerRecord.TableName,
                CreateStatement = new CodePiece { Code = triggerRecord.TriggerCode.NormalizeLineEndings() },
            };
        }
    }
}
