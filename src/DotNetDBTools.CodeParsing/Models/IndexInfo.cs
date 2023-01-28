using System.Collections.Generic;

namespace DotNetDBTools.CodeParsing.Models;

public class IndexInfo : ObjectInfo
{
    public string Table { get; set; }
    public bool Unique { get; set; }
    public List<string> Columns { get; set; } = new();
    public List<string> IncludeColumns { get; set; } = new();
    public string Method { get; set; }
    public string Expression { get; set; }
}
