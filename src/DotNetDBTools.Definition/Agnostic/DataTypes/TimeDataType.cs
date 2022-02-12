using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Agnostic.DataTypes;

/// <summary>
/// Column is declared with a time type appropriate to the used dbms.
/// <list type="bullet">
/// <item><term>MSSQL</term> Column is declared as 'TIME'.</item>
/// <item><term>MySQL</term> Column is declared as 'TIME'.</item>
/// <item><term>PostgreSQL</term> Column is declared as 'TIME'.</item>
/// <item><term>SQLite</term> Column is declared with 'NUMERIC' affinity.</item>
/// </list>
/// </summary>
public class TimeDataType : IDataType
{
}
