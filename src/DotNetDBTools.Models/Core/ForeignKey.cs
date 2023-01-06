using System.Collections.Generic;

namespace DotNetDBTools.Models.Core;

public class ForeignKey : DbObject
{
    public string ThisTableName { get; set; }
    public List<string> ThisColumnNames { get; set; }
    public string ReferencedTableName { get; set; }
    public List<string> ReferencedTableColumnNames { get; set; }
    public string OnUpdate { get; set; }
    public string OnDelete { get; set; }
}
