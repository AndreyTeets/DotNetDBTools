using DotNetDBTools.Definition.Common;

namespace DotNetDBTools.Definition.PostgreSQL;

public class CheckConstraint : SpecificDbmsCheckConstraint
{
    public CheckConstraint(string id) : base(id) { }
}
