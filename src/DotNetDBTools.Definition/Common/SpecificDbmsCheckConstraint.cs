using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Common;

public abstract class SpecificDbmsCheckConstraint : BaseCheckConstraint
{
    protected SpecificDbmsCheckConstraint(string id) : base(id) { }

    /// <summary>
    /// Will appear in sql as 'CHECK (Expression)'
    /// </summary>
    public string Expression { get; set; }
}
