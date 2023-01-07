using DotNetDBTools.Definition.Common;

namespace DotNetDBTools.Definition.PostgreSQL;

public class VerbatimDataType : SpecificDbmsVerbatimDataType
{
    /// <summary>
    /// Name is normalized during database build in the following way:
    /// <list type="bullet">
    /// <item>if it is null or empty exception is thrown.</item>
    /// <item>white space is removed.</item>
    /// <item>if it is a standard sql type resulting name is converted to upper case.</item>
    /// <item>else resulting name stays as is.</item>
    /// </list>
    /// </summary>
    public VerbatimDataType(string name) : base(name) { }
}
