namespace DotNetDBTools.Definition.Core;

public class Expression
{
    public string Code { get; private set; }

    public Expression(string code)
    {
        Code = code;
    }
}
