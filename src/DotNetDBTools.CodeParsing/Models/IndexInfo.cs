using System.Collections.Generic;

namespace DotNetDBTools.CodeParsing.Models;

public class IndexInfo : ObjectInfo
{
    public string Table { get; set; }
    public bool Unique { get; set; }
    public List<string> Columns { get; set; } = new();
}
