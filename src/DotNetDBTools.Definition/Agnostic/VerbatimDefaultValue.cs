using System;
using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Agnostic;

public class VerbatimDefaultValue : IDefaultValue
{
    public Func<DbmsKind, string> Expression { get; private set; }

    public VerbatimDefaultValue(Func<DbmsKind, string> expression)
    {
        Expression = expression;
    }
}
