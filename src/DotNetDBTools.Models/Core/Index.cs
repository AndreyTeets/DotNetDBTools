using System.Collections.Generic;

namespace DotNetDBTools.Models.Core;

public class Index : DbObject
{
    public IEnumerable<string> Columns { get; set; }
    public IEnumerable<string> IncludeColumns { get; set; } = new List<string>();
    public bool Unique { get; set; }
}
