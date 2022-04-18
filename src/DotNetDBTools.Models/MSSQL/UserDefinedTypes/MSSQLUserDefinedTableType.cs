using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MSSQL.UserDefinedTypes;

public class MSSQLUserDefinedTableType : DbObject
{
    public List<Column> Columns { get; set; }
    public PrimaryKey PrimaryKey { get; set; }
    public List<UniqueConstraint> UniqueConstraints { get; set; }
    public List<ForeignKey> ForeignKeys { get; set; }
}
