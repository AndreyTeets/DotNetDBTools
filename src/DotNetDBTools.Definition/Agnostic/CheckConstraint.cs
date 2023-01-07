using System;
using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Agnostic;

public class CheckConstraint : BaseCheckConstraint
{
    public CheckConstraint(string id) : base(id) { }

    /// <summary>
    /// Will appear in sql as 'CHECK (Expression)'
    /// </summary>
    public Func<DbmsKind, string> Expression { get; set; }
}
