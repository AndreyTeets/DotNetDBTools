using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Common;

public abstract class SpecificDbmsVerbatimDefaultValue : IDefaultValue
{
    public string Value { get; private set; }

    protected SpecificDbmsVerbatimDefaultValue(string value)
    {
        Value = value;
    }
}
