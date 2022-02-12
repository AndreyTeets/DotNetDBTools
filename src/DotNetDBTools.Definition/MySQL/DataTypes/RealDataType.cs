using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MySQL.DataTypes;

/// <summary>
/// Column is declared as 'FLOAT'/'DOUBLE'.
/// </summary>
public class RealDataType : IDataType
{
    /// <remarks>
    /// Default value is true.
    /// </remarks>
    public bool IsDoublePrecision { get; set; } = true;
}
