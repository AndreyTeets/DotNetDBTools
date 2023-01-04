using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Common;

public abstract class SpecificDbmsVerbatimDataType : IDataType
{
    public string Name { get; private set; }

    protected SpecificDbmsVerbatimDataType(string name)
    {
        Name = name;
    }
}
