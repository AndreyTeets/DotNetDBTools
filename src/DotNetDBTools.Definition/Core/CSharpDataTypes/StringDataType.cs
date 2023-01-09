namespace DotNetDBTools.Definition.Core.CSharpDataTypes;

/// <summary>
/// Column is declared with a string type appropriate to the used DBMS.
/// <list type="bullet">
/// <item><term>MSSQL</term> Column is declared as 'NCHAR(Length)'/'NVARCHAR(Length/MAX)'.</item>
/// <item><term>MySQL</term> Column is declared as 'CHAR(Length)'/'VARCHAR(Length)'/'LONGTEXT'.</item>
/// <item><term>PostgreSQL</term> Column is declared as 'CHAR(Length)'/'VARCHAR(Length)'/'TEXT'.</item>
/// <item><term>SQLite</term> Column is declared with 'TEXT' affinity.</item>
/// </list>
/// </summary>
public class StringDataType : IDataType
{
    /// <summary>
    /// Length declared for the type if the used DBMS supports it.
    /// <list type="bullet">
    /// <item><term>MSSQL</term> Column is declared as 'NCHAR(Length)'/'NVARCHAR(Length)' if Length is in range [1,4000] otherwise as 'NVARCHAR(MAX)'</item>
    /// <item><term>MySQL</term> Column is declared as 'CHAR(Length)'/'VARCHAR(Length)' if Length is in range [1,255]/[1,65535], otherwise as 'LONGTEXT'.</item>
    /// <item><term>PostgreSQL</term> Column is declared as 'CHAR(Length)'/'VARCHAR(Length)' if Length is positive, otherwise as 'TEXT'</item>
    /// <item><term>SQLite</term> Property is ignored.</item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// Default value is -1.
    /// </remarks>
    public int Length { get; set; } = -1;

    /// <summary>
    /// Controls the exact binary type chosen for the used DBMS.
    /// <list type="bullet">
    /// <item><term>MSSQL</term> Column is declared as 'NCHAR' if IsFixedLength=true and specified Length is in allowed range, otherwise as 'NVARCHAR'.</item>
    /// <item><term>MySQL</term> If specified Length isn't in allowed range property is ignored and column is declared as 'LONGTEXT'.<br/>
    /// Otherwise Column is declared as 'CHAR' if IsFixedLength=true, 'VARCHAR' if IsFixedLength=false.</item>
    /// <item><term>PostgreSQL</term> If specified Length is not positive property is ignored and column is declared as 'TEXT'.<br/>
    /// Otherwise Column is declared as 'CHAR' if IsFixedLength=true, 'VARCHAR' if IsFixedLength=false.</item>
    /// <item><term>SQLite</term> Property is ignored.</item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// Default value is false.
    /// </remarks>
    public bool IsFixedLength { get; set; } = false;
}
