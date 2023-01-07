using DotNetDBTools.Definition.Common;

namespace DotNetDBTools.Definition.MSSQL;

public class VerbatimDefaultValue : SpecificDbmsVerbatimDefaultValue
{
    /// <inheritdoc />
    public VerbatimDefaultValue(string expression) : base(expression) { }
}
