using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MSSQL.UserDefinedTypes;

public class MSSQLUserDefinedTableType : DbObject
{
    public IEnumerable<Column> Columns { get; set; }
    public PrimaryKey PrimaryKey { get; set; }
    public IEnumerable<UniqueConstraint> UniqueConstraints { get; set; }
    public IEnumerable<ForeignKey> ForeignKeys { get; set; }
}
