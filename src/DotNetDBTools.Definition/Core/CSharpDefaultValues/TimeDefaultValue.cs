using System;
using System.Globalization;

namespace DotNetDBTools.Definition.Core.CSharpDefaultValues;

/// <summary>
/// Default value is declared as time according to the used dbms.
/// <list type="bullet">
/// <item><term>MSSQL</term> Default value is declared as TODO.</item>
/// <item><term>MySQL</term> Default value is declared as TODO.</item>
/// <item><term>PostgreSQL</term> Default value is declared as TODO.</item>
/// <item><term>SQLite</term> Default value is declared as TODO.</item>
/// </list>
/// </summary>
public class TimeDefaultValue : IDefaultValue
{
    /// <summary>
    /// Controls how datetime default value is declared.
    /// </summary>
    /// <remarks>
    /// Default value is <see cref="TimeDeclareKind.ISO8601String"/>.
    /// </remarks>
    public TimeDeclareKind DeclareKind { get; set; } = TimeDeclareKind.ISO8601String;
    public TimeSpan Value { get; private set; }

    public TimeDefaultValue(TimeSpan value)
    {
        Value = value;
    }

    public TimeDefaultValue(DateTime value)
    {
        Value = value.TimeOfDay;
    }

    /// <summary>
    /// Constructs value from a time-string formatted as HH:mm:ss.
    /// </summary>
    public TimeDefaultValue(string value)
    {
        Value = TimeSpan.ParseExact(value, "hh':'mm':'ss", CultureInfo.InvariantCulture);
    }
}

public enum TimeDeclareKind
{
    ISO8601String,
    JulianDayNumbers,
    UnixTime,
}
