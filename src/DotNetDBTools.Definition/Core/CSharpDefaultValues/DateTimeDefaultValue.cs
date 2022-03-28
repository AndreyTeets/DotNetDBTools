using System;
using System.Globalization;

namespace DotNetDBTools.Definition.Core.CSharpDefaultValues;

/// <summary>
/// Default value is declared as datetime according to the used dbms.
/// <list type="bullet">
/// <item><term>MSSQL</term> Default value is declared as TODO.</item>
/// <item><term>MySQL</term> Default value is declared as TODO.</item>
/// <item><term>PostgreSQL</term> Default value is declared as TODO.</item>
/// <item><term>SQLite</term> Default value is declared as TODO.</item>
/// </list>
/// </summary>
public class DateTimeDefaultValue : IDefaultValue
{
    /// <summary>
    /// Controls how datetime default value is declared.
    /// </summary>
    /// <remarks>
    /// Default value is <see cref="DateDeclareKind.ISO8601String"/>.
    /// </remarks>
    public DateDeclareKind DeclareKind { get; set; } = DateDeclareKind.ISO8601String;
    public DateTimeOffset Value { get; private set; }
    public bool IsWithTimeZone { get; private set; }

    public DateTimeDefaultValue(DateTimeOffset value)
    {
        Value = value;
    }

    public DateTimeDefaultValue(DateTime value)
    {
        Value = value;
    }

    /// <summary>
    /// Constructs value from a datetime-string formatted as yyyy-MM-dd HH:mm:ss or yyyy-MM-dd HH:mm:sszzz.
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

public enum DateDeclareKind
{
    ISO8601String,
    JulianDayNumbers,
    UnixTime,
}
