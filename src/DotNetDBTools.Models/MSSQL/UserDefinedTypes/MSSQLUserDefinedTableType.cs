using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MSSQL.UserDefinedTypes;

public class MSSQLUserDefinedTableType : DbObject
{
    public List<Column> Columns { get; set; } = new();
    public PrimaryKey PrimaryKey { get; set; }
    public List<UniqueConstraint> UniqueConstraints { get; set; } = new();
    public List<ForeignKey> ForeignKeys { get; set; } = new();
}
