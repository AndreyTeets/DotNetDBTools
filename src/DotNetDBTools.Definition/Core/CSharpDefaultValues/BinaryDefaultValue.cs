using System;

namespace DotNetDBTools.Definition.Core.CSharpDefaultValues;

/// <summary>
/// Default value is declared as binary according to the used dbms.
/// <list type="bullet">
/// <item><term>MSSQL</term> Default value is declared as 0xBytes.</item>
/// <item><term>MySQL</term> Default value is declared as (0xBytes).</item>
/// <item><term>PostgreSQL</term> Default value is declared as '\xBytes'.</item>
/// <item><term>SQLite</term> Default value is declared as X'Bytes'.</item>
/// </list>
/// </summary>
public class BinaryDefaultValue : IDefaultValue
{
    public byte[] Value { get; private set; }

    public BinaryDefaultValue(byte[] value)
    {
        Value = value;
    }

    /// <summary>
    /// Constructs value from a hex-string (e.g. 0xAFD1230E3B).
    /// </summary>
    public BinaryDefaultValue(string value)
    {
        Value = ToByteArray(value);
    }

    private static byte[] ToByteArray(string val)
    {
        string hex = val.Substring(2, val.Length - 2);
        int numChars = hex.Length;
        byte[] bytes = new byte[numChars / 2];
        for (int i = 0; i < numChars; i += 2)
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        return bytes;
    }
}
