using System.Collections.Generic;

namespace DotNetDBTools.Models.Core;

public abstract class Index : DbObject
{
    public string TableName { get; set; }
    public List<string> Columns { get; set; } = new();
    public List<string> IncludeColumns { get; set; } = new();
    public bool Unique { get; set; }
}
