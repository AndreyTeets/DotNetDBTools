using System.Collections.Generic;

namespace DotNetDBTools.Models.Core;

public abstract class TableDiff : DbObjectDiff
{
    public List<Column> ColumnsToAdd { get; set; } = new();
    public List<Column> ColumnsToDrop { get; set; } = new();
    public List<ColumnDiff> ColumnsToAlter { get; set; } = new();

    public PrimaryKey PrimaryKeyToCreate { get; set; }
    public PrimaryKey PrimaryKeyToDrop { get; set; }

    public List<UniqueConstraint> UniqueConstraintsToCreate { get; set; } = new();
    public List<UniqueConstraint> UniqueConstraintsToDrop { get; set; } = new();

    public List<CheckConstraint> CheckConstraintsToCreate { get; set; } = new();
    public List<CheckConstraint> CheckConstraintsToDrop { get; set; } = new();

    public List<ForeignKey> ForeignKeysToCreate { get; set; } = new();
    public List<ForeignKey> ForeignKeysToDrop { get; set; } = new();
}
