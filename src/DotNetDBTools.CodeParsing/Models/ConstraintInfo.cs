using System.Collections.Generic;

namespace DotNetDBTools.CodeParsing.Models;

public class ConstraintInfo : ObjectInfo
{
    public ConstraintType Type { get; set; }
    public List<string> Columns { get; set; } = new();
    public string RefTable { get; set; }
    public List<string> RefColumns { get; set; } = new();
    public string UpdateAction { get; set; }
    public string DeleteAction { get; set; }
    public string Expression { get; set; }
}
