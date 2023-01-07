using DotNetDBTools.Definition.Common;

namespace DotNetDBTools.Definition.MySQL;

public class VerbatimDataType : SpecificDbmsVerbatimDataType
{
    /// <summary>
    /// Name is normalized during database build in the following way:
    /// <list type="bullet">
    /// <item>if it is null or empty exception is thrown.</item>
    /// <item>white space is removed and then converted to upper case.</item>
    /// </list>
    /// </summary>
    public VerbatimDataType(string name) : base(name) { }
}
