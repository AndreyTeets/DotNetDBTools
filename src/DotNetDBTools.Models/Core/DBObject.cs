using System;
using System.Collections.Generic;

namespace DotNetDBTools.Models.Core;

public abstract class DbObject
{
    public Guid ID { get; set; }
    public string Name { get; set; }

    public DbObject Parent { get; set; }

    public List<DbObject> DependsOn { get; set; }
    public List<DbObject> IsDependencyOf { get; set; }
}
