namespace DotNetDBTools.Definition.Core.CSharpDefaultValues;

/// <summary>
/// Default value is declared as string according to the used dbms.
/// <list type="bullet">
/// <item><term>MSSQL</term> Default value is declared as 'String'.</item>
/// <item><term>MySQL</term> Default value is declared as 'String'.</item>
/// <item><term>PostgreSQL</term> Default value is declared as 'String'.</item>
/// <item><term>SQLite</term> Default value is declared as 'String'.</item>
/// </list>
/// </summary>
public class StringDefaultValue : IDefaultValue
{
    public string Value { get; private set; }

    public StringDefaultValue(string value)
    {
        Value = value;
    }
}
