using System;
using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Agnostic;

public class VerbatimDataType : IDataType
{
    public Func<DbmsKind, string> Name { get; private set; }

    /// <summary>
    /// Name is normalized during database conversion to specific DBMS in the following way:
    /// <list type="bullet">
    /// <item><term>MSSQL</term>
    /// <list type="bullet">
    /// <item>if it is null or empty exception is thrown.</item>
    /// <item>white space is removed.</item>
    /// <item>if it is a standard sql type resulting name is converted to upper case.</item>
    /// <item>else resulting name stays as is.</item>
    /// </list>
    /// </item>
    /// <item><term>MySQL</term>
    /// <list type="bullet">
    /// <item>if it is null or empty exception is thrown.</item>
    /// <item>white space is removed and then converted to upper case.</item>
    /// </list>
    /// </item>
    /// <item><term>PostgreSQL</term>
    /// <list type="bullet">
    /// <item>if it is null or empty exception is thrown.</item>
    /// <item>white space is removed.</item>
    /// <item>if it is a standard sql type then resulting name white space is removed, changed with non-alias and converted to upper case.</item>
    /// <item>else resulting name stays as is.</item>
    /// </list>
    /// </item>
    /// <item><term>SQLite</term>
    /// <list type="bullet">
    /// <item>if it is null or empty exception is thrown.</item>
    /// <item>white space is removed and then converted to upper case.</item>
    /// <item>if it contains 'INT' then resulting name is 'INTEGER'.</item>
    /// <item>else if it contains 'CHAR'|'CLOB'|'TEXT' then resulting name is 'TEXT'.</item>
    /// <item>else if it contains 'BLOB' then resulting name is 'BLOB'.</item>
    /// <item>else if it contains 'REAL'|'FLOA'|'DOUB' then resulting name is 'REAL'.</item>
    /// <item>else resulting name is 'NUMERIC'.</item>
    /// </list>
    /// </item>
    /// </list>
    /// </summary>
    public VerbatimDataType(Func<DbmsKind, string> name)
    {
        Name = name;
    }
}
