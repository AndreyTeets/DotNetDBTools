using System.Collections.Generic;

namespace DotNetDBTools.Models.Core;

public abstract class Database
{
    public DatabaseKind Kind { get; protected set; }
    public long Version { get; set; } = 0;
    public IEnumerable<Table> Tables { get; set; }
    public IEnumerable<View> Views { get; set; }
    public IEnumerable<Script> Scripts { get; set; }
}
