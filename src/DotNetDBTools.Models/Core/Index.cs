using System.Collections.Generic;

namespace DotNetDBTools.Models.Core;

public abstract class Index : DbObject
{
    public List<string> Columns { get; set; } = new();
    public List<string> IncludeColumns { get; set; } = new();
    public bool Unique { get; set; }

    public List<Column> DependsOn { get; set; } = new();
}
