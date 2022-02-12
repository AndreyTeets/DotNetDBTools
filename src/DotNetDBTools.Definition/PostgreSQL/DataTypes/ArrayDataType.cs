using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.PostgreSQL.DataTypes;

/// <summary>
/// Column is declared as 'UnderlyingType[Dimensions[0]][Dimensions[1]]...'.
/// </summary>
public class ArrayDataType : IDataType
{
    public IDataType UnderlyingType { get; set; }
    public int[] Dimensions { get; set; } = { 0 };
}
