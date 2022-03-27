using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Common;

public class SpecificDbmsVerbatimDataType : IDataType
{
    public string Name { get; private set; }

    public SpecificDbmsVerbatimDataType(string name)
    {
        Name = name;
    }
}
