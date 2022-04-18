using System.Collections.Generic;

namespace DotNetDBTools.Models.Core;

public abstract class TableDiff
{
    public Table NewTable { get; set; }
    public Table OldTable { get; set; }

    public List<Column> AddedColumns { get; set; }
    public List<Column> RemovedColumns { get; set; }
    public List<ColumnDiff> ChangedColumns { get; set; }

    public PrimaryKey PrimaryKeyToCreate { get; set; }
    public PrimaryKey PrimaryKeyToDrop { get; set; }

    public List<UniqueConstraint> UniqueConstraintsToCreate { get; set; }
    public List<UniqueConstraint> UniqueConstraintsToDrop { get; set; }

    public List<CheckConstraint> CheckConstraintsToCreate { get; set; }
    public List<CheckConstraint> CheckConstraintsToDrop { get; set; }

    public List<Index> IndexesToCreate { get; set; }
    public List<Index> IndexesToDrop { get; set; }

    public List<Trigger> TriggersToCreate { get; set; }
    public List<Trigger> TriggersToDrop { get; set; }

    public List<ForeignKey> ForeignKeysToCreate { get; set; }
    public List<ForeignKey> ForeignKeysToDrop { get; set; }
}
