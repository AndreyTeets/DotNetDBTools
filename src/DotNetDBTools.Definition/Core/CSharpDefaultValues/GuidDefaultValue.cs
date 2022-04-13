using System;

namespace DotNetDBTools.Definition.Core.CSharpDefaultValues;

/// <summary>
/// Default value is declared as guid according to the used DBMS.
/// <list type="bullet">
/// <item><term>MSSQL</term> Default value is declared as 'GuidString'.</item>
/// <item><term>MySQL</term> Default value is declared as (0xGuidBytes).</item>
/// <item><term>PostgreSQL</term> Default value is declared as 'GuidString'.</item>
/// <item><term>SQLite</term> Default value is declared as X'GuidBytes'.</item>
/// </list>
/// </summary>
public class GuidDefaultValue : IDefaultValue
{
    /// <summary>
    /// Controls how guid is converted to byte array for MySQL and SQLite.
    /// For MSSQL and PostgreSQL property is ignored.
    /// </summary>
    /// <remarks>
    /// Default value is <see cref="GuidByteOrder.BigEndian"/>.
    /// </remarks>
    public GuidByteOrder ByteOrder { get; set; } = GuidByteOrder.BigEndian;

    public Guid Value { get; private set; }

    public GuidDefaultValue(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Constructs value from a guid-string that c# Guid constructor can understand.
    /// </summary>
    public GuidDefaultValue(string value)
    {
        Value = new Guid(value);
    }
}

public enum GuidByteOrder
{
    /// <summary>
    /// GuidBytes are produced as GuidValue.ToString().Replace("-", "").ToLower().
    /// </summary>
    BigEndian,
}
