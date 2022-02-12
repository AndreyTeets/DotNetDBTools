using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MSSQL.DataTypes;

/// <summary>
/// Column is declared as 'REAL'/'FLOAT'.
/// </summary>
public class RealDataType : IDataType
{
    /// <remarks>
    /// Default value is true.
    /// </remarks>
    public bool IsDoublePrecision { get; set; } = true;
}
