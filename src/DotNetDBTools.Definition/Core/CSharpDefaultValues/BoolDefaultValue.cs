namespace DotNetDBTools.Definition.Core.CSharpDefaultValues;

/// <summary>
/// Default value is declared as bool according to the used dbms.
/// <list type="bullet">
/// <item><term>MSSQL</term> Default value is declared as 1/0.</item>
/// <item><term>MySQL</term> Default value is declared as 1/0.</item>
/// <item><term>PostgreSQL</term> Default value is declared as TRUE/FALSE.</item>
/// <item><term>SQLite</term> Default value is declared as TRUE/FALSE.</item>
/// </list>
/// </summary>
public class BoolDefaultValue : IDefaultValue
{
    public bool Value { get; private set; }

    public BoolDefaultValue(bool value)
    {
        Value = value;
    }
}
