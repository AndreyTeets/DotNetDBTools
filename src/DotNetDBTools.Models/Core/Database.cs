using System.Collections.Generic;

namespace DotNetDBTools.Models.Core;

public abstract class Database
{
    public DatabaseKind Kind { get; protected set; }
    public long Version { get; set; } = 0;
    public List<Table> Tables { get; set; }
    public List<View> Views { get; set; }
    public List<Script> Scripts { get; set; }

    public void InitializeProperties()
    {
        Tables = new();
        Views = new();
        Scripts = new();
        InitializeAdditionalProperties();
    }
    public virtual void InitializeAdditionalProperties() { }
}
