using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;

internal abstract class GetForeignKeysFromDBMSSysInfoQuery : IQuery
{
    public abstract string Sql { get; }
    public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    public abstract RecordMapper Mapper { get; }

    public class ForeignKeyRecord
    {
        public string ThisTableName { get; set; }
        public string ForeignKeyName { get; set; }
        public string ThisColumnName { get; set; }
        public int ThisColumnPosition { get; set; }
        public string ReferencedTableName { get; set; }
        public string ReferencedColumnName { get; set; }
        public int ReferencedColumnPosition { get; set; }
        public string OnUpdate { get; set; }
        public string OnDelete { get; set; }
    }

    public abstract class RecordMapper
    {
        public abstract ForeignKey MapExceptColumnsToForeignKeyModel(ForeignKeyRecord fkr);
    }
}
