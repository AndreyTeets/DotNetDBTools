using DotNetDBTools.Definition.Common;

namespace DotNetDBTools.Definition.MSSQL;

public class VerbatimDataType : SpecificDbmsVerbatimDataType
{
    /// <inheritdoc />
    public VerbatimDataType(string name) : base(name) { }
}
