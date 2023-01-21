using System;

namespace DotNetDBTools.Models.Core;

public abstract class DbObject
{
    public Guid ID { get; set; }
    public string Name { get; set; }

    public DbObject Parent { get; set; }
}
