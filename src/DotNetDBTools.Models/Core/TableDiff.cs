using System.Collections.Generic;

namespace DotNetDBTools.Models.Core
{
    public abstract class TableDiff
    {
        public TableInfo NewTable { get; set; }
        public TableInfo OldTable { get; set; }

        public IEnumerable<ColumnInfo> AddedColumns { get; set; }
        public IEnumerable<ColumnInfo> RemovedColumns { get; set; }
        public IEnumerable<ColumnDiff> ChangedColumns { get; set; }

        public PrimaryKeyInfo AddedPrimaryKey { get; set; }
        public PrimaryKeyInfo RemovedPrimaryKey { get; set; }

        public IEnumerable<UniqueConstraintInfo> AddedUniqueConstraints { get; set; }
        public IEnumerable<UniqueConstraintInfo> RemovedUniqueConstraints { get; set; }

        public IEnumerable<ForeignKeyInfo> AddedForeignKeys { get; set; }
        public IEnumerable<ForeignKeyInfo> RemovedForeignKeys { get; set; }
    }
}
