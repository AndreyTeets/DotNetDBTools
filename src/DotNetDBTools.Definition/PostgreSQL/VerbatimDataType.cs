using DotNetDBTools.Definition.Common;

namespace DotNetDBTools.Definition.PostgreSQL;

public class VerbatimDataType : SpecificDbmsVerbatimDataType
{
    public VerbatimDataType(string name) : base(name) { }
}
