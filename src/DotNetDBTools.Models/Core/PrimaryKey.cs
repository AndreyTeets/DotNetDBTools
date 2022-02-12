using System.Collections.Generic;

namespace DotNetDBTools.Models.Core;

public class PrimaryKey : DbObject
{
    public IEnumerable<string> Columns { get; set; }
}
