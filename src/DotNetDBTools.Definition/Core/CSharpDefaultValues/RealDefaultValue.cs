using System.Globalization;

namespace DotNetDBTools.Definition.Core.CSharpDefaultValues;

/// <summary>
/// Default value is declared as real according to the used dbms.
/// <list type="bullet">
/// <item><term>MSSQL</term> Default value is declared as Number.</item>
/// <item><term>MySQL</term> Default value is declared as Number.</item>
/// <item><term>PostgreSQL</term> Default value is declared as Number.</item>
/// <item><term>SQLite</term> Default value is declared as Number.</item>
/// </list>
/// </summary>
public class RealDefaultValue : IDefaultValue
{
    public double Value { get; private set; }

    public RealDefaultValue(double value)
    {
        Value = value;
    }

    /// <summary>
    /// Constructs value from a number-string that c# double can parse with invariant culture.
    /// </summary>
    public RealDefaultValue(string value)
    {
        Value = double.Parse(value, CultureInfo.InvariantCulture);
    }
}
