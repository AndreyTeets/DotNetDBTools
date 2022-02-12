using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;

internal abstract class GetTriggersFromDBMSSysInfoQuery : IQuery
{
    public abstract string Sql { get; }
    public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    public abstract RecordMapper Mapper { get; }

    public class TriggerRecord
    {
        public string TableName { get; set; }
        public string TriggerName { get; set; }
        public string TriggerCode { get; set; }
    }

    public abstract class RecordMapper
    {
        public abstract Trigger MapToTriggerModel(TriggerRecord triggerRecord);
    }
}
