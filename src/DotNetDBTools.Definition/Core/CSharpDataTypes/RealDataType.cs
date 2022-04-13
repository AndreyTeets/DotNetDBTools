namespace DotNetDBTools.Definition.Core.CSharpDataTypes;

/// <summary>
/// Column is declared with a real type appropriate to the used DBMS.
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
    /// Default value is false.
    /// </remarks>
    public bool IsSinglePrecision { get; set; } = false;
}
