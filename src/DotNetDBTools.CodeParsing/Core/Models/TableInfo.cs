using System.Collections.Generic;

namespace DotNetDBTools.CodeParsing.Core.Models
{
    public class TableInfo : ObjectInfo
    {
        public List<ColumnInfo> Columns { get; set; } = new();
        public List<ConstraintInfo> Constraints { get; set; } = new();
    }
}
