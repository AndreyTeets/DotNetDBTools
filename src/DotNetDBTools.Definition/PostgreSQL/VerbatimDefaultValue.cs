using DotNetDBTools.Definition.Common;

namespace DotNetDBTools.Definition.PostgreSQL;

public class VerbatimDefaultValue : SpecificDbmsVerbatimDefaultValue
{
    public VerbatimDefaultValue(string value) : base(value) { }
}
