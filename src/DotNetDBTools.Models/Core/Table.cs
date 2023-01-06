using System.Collections.Generic;

namespace DotNetDBTools.Models.Core;

public abstract class Table : DbObject
{
    public List<Column> Columns { get; set; } = new();
    public PrimaryKey PrimaryKey { get; set; }
    public List<UniqueConstraint> UniqueConstraints { get; set; } = new();
    public List<CheckConstraint> CheckConstraints { get; set; } = new();
    public List<ForeignKey> ForeignKeys { get; set; } = new();
    public List<Index> Indexes { get; set; } = new();
    public List<Trigger> Triggers { get; set; } = new();
}
