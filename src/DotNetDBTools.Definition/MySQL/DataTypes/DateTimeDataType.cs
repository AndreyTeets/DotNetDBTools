using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MySQL.DataTypes;

/// <summary>
/// Column is declared as 'DATETIME'/'TIMESTAMP'.
/// </summary>
public class DateTimeDataType : IDataType
{
    /// <remarks>
    /// Default value is <see cref="DateTimeSqlType.DATETIME"/>.
    /// </remarks>
    public DateTimeSqlType SqlType { get; set; } = DateTimeSqlType.DATETIME;
}

public enum DateTimeSqlType
{
    DATETIME,
    TIMESTAMP,
}
