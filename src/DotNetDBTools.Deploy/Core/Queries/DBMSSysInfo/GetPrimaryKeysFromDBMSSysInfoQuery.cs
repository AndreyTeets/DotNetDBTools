using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;

internal abstract class GetPrimaryKeysFromDBMSSysInfoQuery : NoParametersQuery
{
    public abstract RecordMapper Mapper { get; }

    public class PrimaryKeyRecord
    {
        public string TableName { get; set; }
        public string ConstraintName { get; set; }
        public string ColumnName { get; set; }
        public int ColumnPosition { get; set; }
    }

    public abstract class RecordMapper
    {
        public abstract PrimaryKey MapExceptColumnsToPrimaryKeyModel(PrimaryKeyRecord pkr);
    }
}
