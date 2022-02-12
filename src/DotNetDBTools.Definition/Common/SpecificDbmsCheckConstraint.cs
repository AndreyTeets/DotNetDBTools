using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Common;

public abstract class SpecificDbmsCheckConstraint : BaseCheckConstraint
{
    protected SpecificDbmsCheckConstraint(string id) : base(id) { }

    public string Code { get; set; }
}
