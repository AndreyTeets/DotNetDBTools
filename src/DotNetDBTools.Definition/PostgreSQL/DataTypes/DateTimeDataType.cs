using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.PostgreSQL.DataTypes;

/// <summary>
/// Column is declared as 'TIMESTAMP(Precision)'/'TIMESTAMPTZ(Precision)'.
/// </summary>
public class DateTimeDataType : IDataType
{
    /// <remarks>
    /// Default value is 6.
    /// </remarks>
    public byte Precision { get; set; } = 6;

    /// <remarks>
    /// Default value is false.
    /// </remarks>
    public bool IsWithTimeZone { get; set; } = false;
}
