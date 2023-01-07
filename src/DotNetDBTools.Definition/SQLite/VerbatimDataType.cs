using DotNetDBTools.Definition.Common;

namespace DotNetDBTools.Definition.SQLite;

public class VerbatimDataType : SpecificDbmsVerbatimDataType
{
    /// <inheritdoc />
    public VerbatimDataType(string name) : base(name) { }
}
