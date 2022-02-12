using DotNetDBTools.Definition.Common;

namespace DotNetDBTools.Definition.SQLite;

public class CheckConstraint : SpecificDbmsCheckConstraint
{
    public CheckConstraint(string id) : base(id) { }
}
