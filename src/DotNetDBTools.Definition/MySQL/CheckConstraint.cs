using DotNetDBTools.Definition.Common;

namespace DotNetDBTools.Definition.MySQL;

public class CheckConstraint : SpecificDbmsCheckConstraint
{
    public CheckConstraint(string id) : base(id) { }
}
