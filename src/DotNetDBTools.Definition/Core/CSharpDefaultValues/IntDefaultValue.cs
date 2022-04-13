using System.Globalization;

namespace DotNetDBTools.Definition.Core.CSharpDefaultValues;

/// <summary>
/// Default value is declared as int according to the used DBMS.
/// <list type="bullet">
/// <item><term>MSSQL</term> Default value is declared as Number.</item>
/// <item><term>MySQL</term> Default value is declared as Number.</item>
/// <item><term>PostgreSQL</term> Default value is declared as Number.</item>
/// <item><term>SQLite</term> Default value is declared as Number.</item>
/// </list>
/// </summary>
public class IntDefaultValue : IDefaultValue
{
    public long Value { get; private set; }

    public IntDefaultValue(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Constructs value from a number-string that c# long can parse with invariant culture.
    /// </summary>
    public IntDefaultValue(string value)
    {
        Value = long.Parse(value, CultureInfo.InvariantCulture);
    }
}
