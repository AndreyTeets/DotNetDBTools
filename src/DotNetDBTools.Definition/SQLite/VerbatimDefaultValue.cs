using DotNetDBTools.Definition.Common;

namespace DotNetDBTools.Definition.SQLite;

public class VerbatimDefaultValue : SpecificDbmsVerbatimDefaultValue
{
    /// <inheritdoc />
    public VerbatimDefaultValue(string expression) : base(expression) { }
}
