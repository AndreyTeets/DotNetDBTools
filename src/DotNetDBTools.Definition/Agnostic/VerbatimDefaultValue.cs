using System;
using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Agnostic;

public class VerbatimDefaultValue : IDefaultValue
{
    public Func<DbmsKind, string> Expression { get; private set; }

    /// <summary>
    /// Expression will appear in sql as is, so it should be quoted appropriately.
    /// </summary>
    public VerbatimDefaultValue(Func<DbmsKind, string> expression)
    {
        Expression = expression;
    }
}
