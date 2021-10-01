using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Agnostic.DataTypes
{
    /// <summary>
    /// Column is declared with a bool type appropriate to the used dbms.
    /// <list type="bullet">
    /// <item><term>SQLite</term> Not supported.</item>
    /// <item><term>MSSQL</term> Column is declared as 'bit'.</item>
    /// </list>
    /// </summary>
    public class BoolDataType : IDataType
    {
    }
}
