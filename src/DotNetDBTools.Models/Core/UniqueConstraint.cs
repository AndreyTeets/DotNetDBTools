using System.Collections.Generic;

namespace DotNetDBTools.Models.Core;

public class UniqueConstraint : DbObject
{
    public List<string> Columns { get; set; }
}
