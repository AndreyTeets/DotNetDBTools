using System.Collections.Generic;

namespace DotNetDBTools.Models.Core;

public class ForeignKey : DbObject
{
    public List<string> ThisColumnNames { get; set; } = new();
    public string ReferencedTableName { get; set; }
    public List<string> ReferencedTableColumnNames { get; set; } = new();
    public string OnUpdate { get; set; }
    public string OnDelete { get; set; }
}
