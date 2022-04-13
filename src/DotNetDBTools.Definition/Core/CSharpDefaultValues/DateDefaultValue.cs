using System;
using System.Globalization;

namespace DotNetDBTools.Definition.Core.CSharpDefaultValues;

/// <summary>
/// Default value is declared as date according to the used DBMS and DeclareKind.
/// </summary>
public class DateDefaultValue : IDefaultValue
{
    /// <summary>
    /// Controls how datetime default value is declared.
    /// </summary>
    /// <remarks>
    /// Default value is <see cref="DateDeclareKind.ISO8601String"/>.
    /// </remarks>
    public DateDeclareKind DeclareKind { get; set; } = DateDeclareKind.ISO8601String;

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

public enum DateDeclareKind
{
    /// <summary>
    /// Default value is declared as ISO8601 date-string according to the used DBMS.
    /// <list type="bullet">
    /// <item><term>MSSQL</term> Default value is declared as 'yyyy-MM-dd'.</item>
    /// <item><term>MySQL</term> Default value is declared as 'yyyy-MM-dd'.</item>
    /// <item><term>PostgreSQL</term> Default value is declared as 'yyyy-MM-dd'.</item>
    /// <item><term>SQLite</term> Default value is declared as 'yyyy-MM-dd'.</item>
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
