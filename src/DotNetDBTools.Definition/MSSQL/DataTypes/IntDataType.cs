using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MSSQL.DataTypes;

/// <summary>
/// Column is declared as 'TINYINT'/'SMALLINT'/'INT'/'BIGINT'.
/// </summary>
public class IntDataType : IDataType
{
    /// <remarks>
    /// Default value is <see cref="IntSize.Int32"/>.
    /// </remarks>
    public IntSize Size { get; set; } = IntSize.Int32;
}

public enum IntSize
{
    Int8,
    Int16,
    Int32,
    Int64,
}
