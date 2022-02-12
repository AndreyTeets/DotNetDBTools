using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Agnostic.DataTypes;

/// <summary>
/// Column is declared with a real type appropriate to the used dbms.
/// <list type="bullet">
/// <item><term>MSSQL</term> Column is declared as 'REAL'/'FLOAT'.</item>
/// <item><term>MySQL</term> Column is declared as 'FLOAT'/'DOUBLE'.</item>
/// <item><term>PostgreSQL</term> Column is declared as 'FLOAT4'/'FLOAT8'.</item>
/// <item><term>SQLite</term> Column is declared with 'REAL' affinity.</item>
/// </list>
/// </summary>
public class RealDataType : IDataType
{
    /// <remarks>
    /// Default value is true.
    /// </remarks>
    public bool IsDoublePrecision { get; set; } = true;
}
