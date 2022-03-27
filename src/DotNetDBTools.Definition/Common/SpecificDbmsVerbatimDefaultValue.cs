using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Common;

public class SpecificDbmsVerbatimDefaultValue : IDefaultValue
{
    public string Value { get; private set; }

    public SpecificDbmsVerbatimDefaultValue(string value)
    {
        Value = value;
    }
}
