using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo
{
    internal abstract class GetUniqueConstraintsFromDBMSSysInfoQuery : IQuery
    {
        public abstract string Sql { get; }
        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
        public abstract RecordMapper Mapper { get; }

        public class UniqueConstraintRecord
        {
            public string TableName { get; set; }
            public string ConstraintName { get; set; }
            public string ColumnName { get; set; }
            public int ColumnPosition { get; set; }
        }

        public abstract class RecordMapper
        {
            public abstract UniqueConstraint MapExceptColumnsToUniqueConstraintModel(UniqueConstraintRecord ucr);
        }
    }
}
