using System;
using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Agnostic;

public class Trigger : BaseTrigger
{
    public Trigger(string id) : base(id) { }

    /// <summary>
    /// Full create trigger statement.
    /// </summary>
    public Func<DbmsKind, string> Code { get; set; }
}
