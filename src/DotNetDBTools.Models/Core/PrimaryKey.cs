using System.Collections.Generic;

namespace DotNetDBTools.Models.Core;

public class PrimaryKey : DbObject
{
    public List<string> Columns { get; set; } = new();
}
