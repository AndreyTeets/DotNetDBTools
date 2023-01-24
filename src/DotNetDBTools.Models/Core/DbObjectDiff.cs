using System;

namespace DotNetDBTools.Models.Core;

public abstract class DbObjectDiff
{
    public Guid ID { get; set; }
    public string NewName { get; set; }
    public string OldName { get; set; }
}
