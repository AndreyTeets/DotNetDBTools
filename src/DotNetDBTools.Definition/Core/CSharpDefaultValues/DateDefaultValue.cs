using System;
using System.Globalization;

namespace DotNetDBTools.Definition.Core.CSharpDefaultValues;

/// <summary>
/// Default value is declared as date according to the used dbms.
/// <list type="bullet">
/// <item><term>MSSQL</term> Default value is declared as TODO.</item>
/// <item><term>MySQL</term> Default value is declared as TODO.</item>
/// <item><term>PostgreSQL</term> Default value is declared as TODO.</item>
/// <item><term>SQLite</term> Default value is declared as TODO.</item>
/// </list>
/// </summary>
public class DateDefaultValue : IDefaultValue
{
    /// <summary>
    /// Controls how datetime default value is declared.
    /// </summary>
    /// <remarks>
    /// Default value is <see cref="DateTimeDeclareKind.ISO8601String"/>.
    /// </remarks>
    public DateTimeDeclareKind DeclareKind { get; set; } = DateTimeDeclareKind.ISO8601String;
    public DateTime Value { get; private set; }

    public DateDefaultValue(DateTime value)
    {
        Value = value.Date;
    }

    /// <summary>
    /// Constructs value from a date-string formatted as yyyy-MM-dd.
    /// </summary>
    public DateDefaultValue(string value)
    {
        Value = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
    }
}

public enum DateTimeDeclareKind
{
    ISO8601String,
    JulianDayNumbers,
    UnixTime,
}
