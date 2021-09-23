using System.Collections.Generic;

namespace DotNetDBTools.Models.Core
{
    public abstract class TableInfo : DBObjectInfo
    {
        public IEnumerable<ColumnInfo> Columns { get; set; }
        public PrimaryKeyInfo PrimaryKey { get; set; }
        public IEnumerable<UniqueConstraintInfo> UniqueConstraints { get; set; }
        public IEnumerable<ForeignKeyInfo> ForeignKeys { get; set; }
    }
}
