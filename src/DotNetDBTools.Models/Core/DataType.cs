using System.Collections.Generic;

namespace DotNetDBTools.Models.Core;

public class DataType
{
    public string Name { get; set; }

    public List<DbObject> DependsOn { get; set; } = new();
}
