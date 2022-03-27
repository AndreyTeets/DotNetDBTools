using DotNetDBTools.Definition.Common;

namespace DotNetDBTools.Definition.MSSQL;

public class VerbatimDefaultValue : SpecificDbmsVerbatimDefaultValue
{
    public VerbatimDefaultValue(string value) : base(value) { }
}
