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

        public PrimaryKeyInfo PrimaryKeyToCreate { get; set; }
        public PrimaryKeyInfo PrimaryKeyToDrop { get; set; }

        public IEnumerable<UniqueConstraintInfo> UniqueConstraintsToCreate { get; set; }
        public IEnumerable<UniqueConstraintInfo> UniqueConstraintsToDrop { get; set; }

        public IEnumerable<CheckConstraintInfo> CheckConstraintsToCreate { get; set; }
        public IEnumerable<CheckConstraintInfo> CheckConstraintsToDrop { get; set; }

        public IEnumerable<ForeignKeyInfo> ForeignKeysToCreate { get; set; }
        public IEnumerable<ForeignKeyInfo> ForeignKeysToDrop { get; set; }

        public IEnumerable<IndexInfo> IndexesToCreate { get; set; }
        public IEnumerable<IndexInfo> IndexesToDrop { get; set; }

        public IEnumerable<TriggerInfo> TriggersToCreate { get; set; }
        public IEnumerable<TriggerInfo> TriggersToDrop { get; set; }
    }
}
