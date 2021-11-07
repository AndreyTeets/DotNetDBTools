using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Agnostic.DataTypes
{
    /// <summary>
    /// Column is declared with a bool type appropriate to the used dbms.
    /// <list type="bullet">
    /// <item><term>MSSQL</term> Column is declared as 'BIT'.</item>
    /// <item><term>MySQL</term> Column is declared as 'TINYINT(1)'.</item>
    /// <item><term>PostgreSQL</term> Column is declared as 'BOOL'.</item>
    /// <item><term>SQLite</term> Column is declared with 'INTEGER' affinity.</item>
    /// </list>
    /// </summary>
    public class BoolDataType : IDataType
    {
    }
}
