using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;

internal abstract class GetCheckConstraintsFromDBMSSysInfoQuery : NoParametersQuery
{
    public abstract RecordMapper Mapper { get; }

    public class CheckConstraintRecord
    {
        public string TableName { get; set; }
        public string ConstraintName { get; set; }
        public string ConstraintCode { get; set; }
    }

    public abstract class RecordMapper
    {
        public abstract CheckConstraint MapToCheckConstraintModel(CheckConstraintRecord ckr);
    }
}
