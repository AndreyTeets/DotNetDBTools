using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Common;

public abstract class SpecificDbmsVerbatimDefaultValue : IDefaultValue
{
    public string Expression { get; private set; }

    protected SpecificDbmsVerbatimDefaultValue(string expression)
    {
        Expression = expression;
    }
}
