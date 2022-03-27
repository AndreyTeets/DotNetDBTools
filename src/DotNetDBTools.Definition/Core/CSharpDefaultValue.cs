using System;

namespace DotNetDBTools.Definition.Core;

public class CSharpDefaultValue : IDefaultValue
{
    public object Value { get; private set; }

    public CSharpDefaultValue(long value)
    {
        Value = value;
    }

    public CSharpDefaultValue(double value)
    {
        Value = value;
    }

    public CSharpDefaultValue(decimal value)
    {
        Value = value;
    }

    public CSharpDefaultValue(bool value)
    {
        Value = value;
    }

    public CSharpDefaultValue(string value)
    {
        Value = value;
    }

    public CSharpDefaultValue(byte[] value)
    {
        Value = value;
    }

    public CSharpDefaultValue(DateTime value)
    {
        Value = value;
    }

    public CSharpDefaultValue(Guid value)
    {
        Value = value;
    }
}
