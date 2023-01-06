using System.Collections.Generic;

namespace DotNetDBTools.Models.Core;

public abstract class TableDiff
{
    public Table NewTable { get; set; }
    public Table OldTable { get; set; }

    public List<Column> AddedColumns { get; set; } = new();
    public List<Column> RemovedColumns { get; set; } = new();
    public List<ColumnDiff> ChangedColumns { get; set; } = new();

    public PrimaryKey PrimaryKeyToCreate { get; set; }
    public PrimaryKey PrimaryKeyToDrop { get; set; }

    public List<UniqueConstraint> UniqueConstraintsToCreate { get; set; } = new();
    public List<UniqueConstraint> UniqueConstraintsToDrop { get; set; } = new();

    public List<CheckConstraint> CheckConstraintsToCreate { get; set; } = new();
    public List<CheckConstraint> CheckConstraintsToDrop { get; set; } = new();

    public List<ForeignKey> ForeignKeysToCreate { get; set; } = new();
    public List<ForeignKey> ForeignKeysToDrop { get; set; } = new();

    public List<Index> IndexesToCreate { get; set; } = new();
    public List<Index> IndexesToDrop { get; set; } = new();

    public List<Trigger> TriggersToCreate { get; set; } = new();
    public List<Trigger> TriggersToDrop { get; set; } = new();
}
