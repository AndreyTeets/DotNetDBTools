using System;
using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Agnostic;

public class CheckConstraint : BaseCheckConstraint
{
    public CheckConstraint(string id) : base(id) { }

    public Func<DbmsKind, string> Code { get; set; }
}
