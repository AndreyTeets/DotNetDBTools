using System.Collections.Generic;

namespace DotNetDBTools.Models.Core;

public abstract class Database
{
    public DatabaseKind Kind { get; protected set; }
    public long Version { get; set; } = 0;
    public List<Table> Tables { get; set; } = new();
    public List<View> Views { get; set; } = new();
    public List<Script> Scripts { get; set; } = new();
}
