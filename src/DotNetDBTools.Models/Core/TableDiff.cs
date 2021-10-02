using System.Collections.Generic;

namespace DotNetDBTools.Models.Core
{
    public abstract class TableDiff
    {
        public Table NewTable { get; set; }
        public Table OldTable { get; set; }

        public IEnumerable<Column> AddedColumns { get; set; }
        public IEnumerable<Column> RemovedColumns { get; set; }
        public IEnumerable<ColumnDiff> ChangedColumns { get; set; }

        public PrimaryKey PrimaryKeyToCreate { get; set; }
        public PrimaryKey PrimaryKeyToDrop { get; set; }

        public IEnumerable<UniqueConstraint> UniqueConstraintsToCreate { get; set; }
        public IEnumerable<UniqueConstraint> UniqueConstraintsToDrop { get; set; }

        public IEnumerable<CheckConstraint> CheckConstraintsToCreate { get; set; }
        public IEnumerable<CheckConstraint> CheckConstraintsToDrop { get; set; }

        public IEnumerable<ForeignKey> ForeignKeysToCreate { get; set; }
        public IEnumerable<ForeignKey> ForeignKeysToDrop { get; set; }

        public IEnumerable<Index> IndexesToCreate { get; set; }
        public IEnumerable<Index> IndexesToDrop { get; set; }

        public IEnumerable<Trigger> TriggersToCreate { get; set; }
        public IEnumerable<Trigger> TriggersToDrop { get; set; }
    }
}
