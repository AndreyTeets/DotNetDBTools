using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Agnostic.DataTypes;

/// <summary>
/// Column is declared with a binary type appropriate to the used dbms.
/// <list type="bullet">
/// <item><term>MSSQL</term> Column is declared as 'BINARY(Length)'/'VARBINARY(Length/MAX)'.</item>
/// <item><term>MySQL</term> Column is declared as 'BINARY(Length)'/'VARBINARY(Length)'/'LONGBLOB'.</item>
/// <item><term>PostgreSQL</term> Column is declared as 'BYTEA'.</item>
/// <item><term>SQLite</term> Column is declared with 'BLOB' affinity.</item>
/// </list>
/// </summary>
public class BinaryDataType : IDataType
{
    /// <summary>
    /// Length declared for the type if the used dbms supports it.
    /// <list type="bullet">
    /// <item><term>MSSQL</term> Column is declared as 'BINARY(Length)'/'VARBINARY(Length)' if Length is in range [1,8000] otherwise as 'VARBINARY(MAX)'</item>
    /// <item><term>MySQL</term> Column is declared as 'BINARY(Length)'/'VARBINARY(Length)' if Length is in range [1,255]/[1,65535], otherwise as 'LONGBLOB'.</item>
    /// <item><term>PostgreSQL</term> Property is ignored.</item>
    /// <item><term>SQLite</term> Property is ignored.</item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// Default value is 50.
    /// </remarks>
    public int Length { get; set; } = 50;

    /// <summary>
    /// Controls the exact binary type chosen for the used dbms.
    /// <list type="bullet">
    /// <item><term>MSSQL</term> Column is declared as 'BINARY' if IsFixedLength=true and specified Length is in allowed range, otherwise as 'VARBINARY'.</item>
    /// <item><term>MySQL</term> If specified Length isn't in allowed range property is ignored and Column is declared as 'LONGBLOB'.<br/>
    /// Otherwise Column is declared as 'BINARY' if IsFixedLength=true, 'VARBINARY' if IsFixedLength=false.</item>
    /// <item><term>PostgreSQL</term> Property is ignored.</item>
    /// <item><term>SQLite</term> Property is ignored.</item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// Default value is false.
    /// </remarks>
    public bool IsFixedLength { get; set; } = false;
}
