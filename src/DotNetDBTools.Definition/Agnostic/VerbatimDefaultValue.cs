using System;
using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Agnostic;

public class VerbatimDefaultValue : IDefaultValue
{
    public Func<DbmsKind, string> Value { get; private set; }

    public VerbatimDefaultValue(Func<DbmsKind, string> value)
    {
        Value = value;
    }
}
