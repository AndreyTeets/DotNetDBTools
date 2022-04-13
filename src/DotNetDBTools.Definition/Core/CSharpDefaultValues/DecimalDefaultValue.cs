using System.Globalization;

namespace DotNetDBTools.Definition.Core.CSharpDefaultValues;

/// <summary>
/// Default value is declared as decimal according to the used DBMS.
/// <list type="bullet">
/// <item><term>MSSQL</term> Default value is declared as Number.</item>
/// <item><term>MySQL</term> Default value is declared as Number.</item>
/// <item><term>PostgreSQL</term> Default value is declared as Number.</item>
/// <item><term>SQLite</term> Default value is declared as Number.</item>
/// </list>
/// </summary>
public class DecimalDefaultValue : IDefaultValue
{
    public decimal Value { get; private set; }

    public DecimalDefaultValue(decimal value)
    {
        Value = value;
    }

    /// <summary>
    /// Constructs value from a number-string that c# decimal can parse with invariant culture.
    /// </summary>
    public DecimalDefaultValue(string value)
    {
        Value = decimal.Parse(value, CultureInfo.InvariantCulture);
    }
}
