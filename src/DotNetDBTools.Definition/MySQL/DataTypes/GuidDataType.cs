using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MySQL.DataTypes;

/// <summary>
/// Column is declared as 'BINARY(16)'/'CHAR(32)'/'CHAR(36)'.
/// </summary>
public class GuidDataType : IDataType
{

    /// <remarks>
    /// Default value is <see cref="GuidSqlType.BINARY16"/>.
    /// </remarks>
    public GuidSqlType SqlType { get; set; } = GuidSqlType.BINARY16;
}

public enum GuidSqlType
{
    BINARY16,
    CHAR32,
    CHAR36,
}
