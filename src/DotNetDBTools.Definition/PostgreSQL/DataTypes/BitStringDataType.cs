using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.PostgreSQL.DataTypes;

/// <summary>
/// Column is declared as 'BIT(Length)'/'VARBIT(Length)'.
/// </summary>
public class BitStringDataType : IDataType
{
    /// <remarks>
    /// Default value is 8.
    /// </remarks>
    public int Length { get; set; } = 8;

    /// <remarks>
    /// Default value is true.
    /// </remarks>
    public bool IsFixedLength { get; set; } = true;
}
