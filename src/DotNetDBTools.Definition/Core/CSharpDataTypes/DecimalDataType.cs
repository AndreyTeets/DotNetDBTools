namespace DotNetDBTools.Definition.Core.CSharpDataTypes;

/// <summary>
/// Column is declared with a decimal type appropriate to the used DBMS.
/// <list type="bullet">
/// <item><term>MSSQL</term> Column is declared as 'DECIMAL(Precision, Scale)'.</item>
/// <item><term>MySQL</term> Column is declared as 'DECIMAL(Precision, Scale)'.</item>
/// <item><term>PostgreSQL</term> Column is declared as 'DECIMAL(Precision, Scale)'.</item>
/// <item><term>SQLite</term> Column is declared with 'NUMERIC' affinity.</item>
/// </list>
/// </summary>
public class DecimalDataType : IDataType
{
    /// <remarks>
    /// Default value is 19.
    /// </remarks>
    public byte Precision { get; set; } = 19;

    /// <remarks>
    /// Default value is 2.
    /// </remarks>
    public byte Scale { get; set; } = 2;
}
