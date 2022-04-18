using System.Collections.Generic;

namespace DotNetDBTools.CodeParsing.Models;

public class TableInfo : ObjectInfo
{
    public List<ColumnInfo> Columns { get; set; } = new();
    public List<ConstraintInfo> Constraints { get; set; } = new();
}
