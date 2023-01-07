using DotNetDBTools.Definition.Common;

namespace DotNetDBTools.Definition.SQLite;

public class VerbatimDataType : SpecificDbmsVerbatimDataType
{
    /// <summary>
    /// Name is normalized during database build in the following way:
    /// <list type="bullet">
    /// <item>if it is null or empty exception is thrown.</item>
    /// <item>white space is removed and then converted to upper case.</item>
    /// <item>if it contains 'INT' then resulting name is 'INTEGER'.</item>
    /// <item>else if it contains 'CHAR'|'CLOB'|'TEXT' then resulting name is 'TEXT'.</item>
    /// <item>else if it contains 'BLOB' then resulting name is 'BLOB'.</item>
    /// <item>else if it contains 'REAL'|'FLOA'|'DOUB' then resulting name is 'REAL'.</item>
    /// <item>else resulting name is 'NUMERIC'.</item>
    /// </list>
    /// </summary>
    public VerbatimDataType(string name) : base(name) { }
}
