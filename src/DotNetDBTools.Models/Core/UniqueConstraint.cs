using System.Collections.Generic;

namespace DotNetDBTools.Models.Core;

public class UniqueConstraint : DbObject
{
    public IEnumerable<string> Columns { get; set; }
}
