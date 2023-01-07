using DotNetDBTools.Definition.Common;

namespace DotNetDBTools.Definition.MySQL;

public class VerbatimDefaultValue : SpecificDbmsVerbatimDefaultValue
{
    public VerbatimDefaultValue(string expression) : base(expression) { }
}
