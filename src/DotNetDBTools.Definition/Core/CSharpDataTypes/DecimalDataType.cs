namespace DotNetDBTools.Definition.Core.CSharpDataTypes;

/// <summary>
/// Column is declared with a decimal type appropriate to the used DBMS.
/// <list type="bullet">
/// <item><term>MSSQL</term> Column is declared as 'DECIMAL(Precision,Scale)'.</item>
/// <item><term>MySQL</term> Column is declared as 'DECIMAL(Precision,Scale)'.</item>
/// <item><term>PostgreSQL</term> Column is declared as 'DECIMAL'/'DECIMAL(Precision)'/'DECIMAL(Precision,Scale)'.</item>
/// <item><term>SQLite</term> Column is declared with 'NUMERIC' affinity.</item>
/// </list>
/// </summary>
public class DecimalDataType : IDataType
{
    /// <summary>
    /// <list type="bullet">
    /// <item><term>MSSQL</term> Non-positive Precision value is treated as 18.</item>
    /// <item><term>MySQL</term> Non-positive Precision value is treated as 10.</item>
    /// <item><term>PostgreSQL</term>
    /// <list type="bullet">
    /// <item>If Precision is positive and Scale is not zero column is declared as 'DECIMAL(Precision,Scale)'.</item>
    /// <item>If Precision is positive and Scale is zero column is declared as 'DECIMAL(Precision)'.</item>
    /// <item>If Precision is not positive Scale property is ignored and column is declared as 'DECIMAL'.</item>
    /// </list>
    /// </item>
    /// <item><term>SQLite</term> Property is ignored.</item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// Default value is -1.
    /// </remarks>
    public int Precision { get; set; } = -1;

    /// <summary>
    /// <list type="bullet">
    /// <item><term>MSSQL</term> Scale is used in declaration as is.</item>
    /// <item><term>MySQL</term> Scale is used in declaration as is.</item>
    /// <item><term>PostgreSQL</term>
    /// <list type="bullet">
    /// <item>If Precision is positive and Scale is not zero column is declared as 'DECIMAL(Precision,Scale)'.</item>
    /// <item>If Precision is positive and Scale is zero column is declared as 'DECIMAL(Precision)'.</item>
    /// <item>If Precision is not positive Scale property is ignored and column is declared as 'DECIMAL'.</item>
    /// </list>
    /// </item>
    /// <item><term>SQLite</term> Property is ignored.</item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// Default value is 0.
    /// </remarks>
    public int Scale { get; set; } = 0;
}
