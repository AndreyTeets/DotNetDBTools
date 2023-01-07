using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Common;

public abstract class SpecificDbmsVerbatimDefaultValue : IDefaultValue
{
    public string Expression { get; private set; }

    /// <summary>
    /// Expression will appear in sql as is, so it should be quoted appropriately.
    /// </summary>
    protected SpecificDbmsVerbatimDefaultValue(string expression)
    {
        Expression = expression;
    }
}
