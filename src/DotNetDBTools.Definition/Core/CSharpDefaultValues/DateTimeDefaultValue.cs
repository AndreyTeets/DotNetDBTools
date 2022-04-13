using System;
using System.Globalization;

namespace DotNetDBTools.Definition.Core.CSharpDefaultValues;

/// <summary>
/// Default value is declared as datetime according to the used DBMS and DeclareKind.
/// </summary>
public class DateTimeDefaultValue : IDefaultValue
{
    /// <summary>
    /// Controls how datetime default value is declared.
    /// </summary>
    /// <remarks>
    /// Default value is <see cref="DateTimeDeclareKind.ISO8601String"/>.
    /// </remarks>
    public DateTimeDeclareKind DeclareKind { get; set; } = DateTimeDeclareKind.ISO8601String;

    public DateTimeOffset Value { get; private set; }
    public bool IsWithTimeZone { get; private set; }

    /// <summary>
    /// value.Offset != TimeSpan.Zero then IsWithTimeZone flag is set.
    /// </summary>
    public DateTimeDefaultValue(DateTimeOffset value)
    {
        Value = value;
        if (value.Offset != TimeSpan.Zero)
            IsWithTimeZone = true;
    }

    /// <summary>
    /// If value.Kind == DateTimeKind.Utc then IsWithTimeZone flag is set.
    /// </summary>
    public DateTimeDefaultValue(DateTime value)
    {
        Value = value;
        if (value.Kind == DateTimeKind.Utc)
            IsWithTimeZone = true;
    }

    /// <summary>
    /// Constructs value from a datetime-string formatted as yyyy-MM-dd HH:mm:ss or yyyy-MM-dd HH:mm:sszzz.
    /// If yyyy-MM-dd HH:mm:sszzz format is used then IsWithTimeZone flag is set.
    /// </summary>
    public DateTimeDefaultValue(string value)
    {
        if (value.Length > 19)
        {
            Value = DateTimeOffset.ParseExact(value, "yyyy-MM-dd HH:mm:sszzz", CultureInfo.InvariantCulture);
            IsWithTimeZone = true;
        }
        else
        {
            Value = DateTimeOffset.ParseExact(value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            IsWithTimeZone = false;
        }
    }
}

public enum DateTimeDeclareKind
{
    /// <summary>
    /// Default value is declared as ISO8601 datetime-string without/with timezone according to the used DBMS.
    /// <list type="bullet">
    /// <item><term>MSSQL</term> Default value is declared as 'yyyy-MM-dd HH:mm:ss'/'yyyy-MM-dd HH:mm:sszzz'.</item>
    /// <item><term>MySQL</term> Default value is declared as 'yyyy-MM-dd HH:mm:ss'/'yyyy-MM-dd HH:mm:ss' (if with timezone it's converted to offset=0 first).</item>
    /// <item><term>PostgreSQL</term> Default value is declared as 'yyyy-MM-dd HH:mm:ss'/'yyyy-MM-dd HH:mm:sszz' (if with timezone it's converted to offset=0 first).</item>
    /// <item><term>SQLite</term> Default value is declared as 'yyyy-MM-dd HH:mm:ss'/'yyyy-MM-dd HH:mm:sszzz'.</item>
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
