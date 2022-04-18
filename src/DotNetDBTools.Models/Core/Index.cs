using System.Collections.Generic;

namespace DotNetDBTools.Models.Core;

public class Index : DbObject
{
    public List<string> Columns { get; set; }
    public List<string> IncludeColumns { get; set; } = new();
    public bool Unique { get; set; }
}
