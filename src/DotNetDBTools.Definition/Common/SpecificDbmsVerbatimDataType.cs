using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Common;

public abstract class SpecificDbmsVerbatimDataType : IDataType
{
    public string Name { get; private set; }

    /// <summary>
    /// Name will appear in sql normalized and automatically quoted if it's required.
    /// </summary>
    protected SpecificDbmsVerbatimDataType(string name)
    {
        Name = name;
    }
}
