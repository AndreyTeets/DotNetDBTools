using DotNetDBTools.Definition.Common;

namespace DotNetDBTools.Definition.MySQL;

public class VerbatimDataType : SpecificDbmsVerbatimDataType
{
    /// <inheritdoc />
    public VerbatimDataType(string name) : base(name) { }
}
