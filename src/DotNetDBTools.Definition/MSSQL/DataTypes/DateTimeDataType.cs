using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MSSQL.DataTypes;

/// <summary>
/// Column is declared as 'SMALLDATETIME'/'DATETIME'/'DATETIME2'/'DATETIMEOFFSET'.
/// </summary>
public class DateTimeDataType : IDataType
{
    /// <remarks>
    /// Default value is <see cref="DateTimeSqlType.DATETIME2"/>.
    /// </remarks>
    public DateTimeSqlType SqlType { get; set; } = DateTimeSqlType.DATETIME2;
}

public enum DateTimeSqlType
{
    SMALLDATETIME,
    DATETIME,
    DATETIME2,
    DATETIMEOFFSET,
}
