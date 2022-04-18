using System.Collections.Generic;

namespace DotNetDBTools.Models.Core;

public abstract class Table : DbObject
{
    public List<Column> Columns { get; set; }
    public PrimaryKey PrimaryKey { get; set; }
    public List<UniqueConstraint> UniqueConstraints { get; set; }
    public List<CheckConstraint> CheckConstraints { get; set; }
    public List<ForeignKey> ForeignKeys { get; set; }
    public List<Index> Indexes { get; set; }
    public List<Trigger> Triggers { get; set; }
}
