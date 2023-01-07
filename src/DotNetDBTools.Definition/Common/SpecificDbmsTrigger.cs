using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Common;

public abstract class SpecificDbmsTrigger : BaseTrigger
{
    protected SpecificDbmsTrigger(string id) : base(id) { }

    /// <summary>
    /// Full create trigger statement.
    /// </summary>
    public string Code { get; set; }
}
