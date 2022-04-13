using System;
using System.Globalization;

namespace DotNetDBTools.Definition.Core.CSharpDefaultValues;

/// <summary>
/// Default value is declared as time according to the used DBMS and DeclareKind.
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
    /// <summary>
    /// Default value is declared as ISO8601 time-string according to the used DBMS.
    /// <list type="bullet">
    /// <item><term>MSSQL</term> Default value is declared as 'HH:mm:ss'.</item>
    /// <item><term>MySQL</term> Default value is declared as 'HH:mm:ss'.</item>
    /// <item><term>PostgreSQL</term> Default value is declared as 'HH:mm:ss'.</item>
    /// <item><term>SQLite</term> Default value is declared as 'HH:mm:ss'.</item>
    /// </list>
    /// </summary>
    ISO8601String,

    /// <summary>
    /// Not implemented.
    /// </summary>
    JulianDayNumbers,

    /// <summary>
    /// Not implemented.
    /// </summary>
    UnixTime,
}
