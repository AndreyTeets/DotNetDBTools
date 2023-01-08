using DotNetDBTools.Definition.Common;

namespace DotNetDBTools.Definition.PostgreSQL;

public class VerbatimDataType : SpecificDbmsVerbatimDataType
{
    /// <summary>
    /// Name is normalized during database build in the following way:
    /// <list type="bullet">
    /// <item>if it is null or empty exception is thrown.</item>
    /// <item>quotes are removed.</item>
    /// <item>if it is a standard sql type then resulting name white space is removed, changed with non-alias and converted to upper case.</item>
    /// <item>else resulting name is quoted.</item>
    /// <item>then white space and array sizes are removed (e.g. 'MyCustomType [ 33] [4 ]' => 'MyCustomType[][]').</item>
    /// </list>
    /// </summary>
    public VerbatimDataType(string name) : base(name) { }
}
