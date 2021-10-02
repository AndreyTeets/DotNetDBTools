using System.Collections.Generic;

namespace DotNetDBTools.Models.Core
{
    public abstract class Table : DBObject
    {
        public IEnumerable<Column> Columns { get; set; }
        public PrimaryKey PrimaryKey { get; set; }
        public IEnumerable<UniqueConstraint> UniqueConstraints { get; set; }
        public IEnumerable<CheckConstraint> CheckConstraints { get; set; }
        public IEnumerable<ForeignKey> ForeignKeys { get; set; }
        public IEnumerable<Index> Indexes { get; set; }
        public IEnumerable<Trigger> Triggers { get; set; }
    }
}
