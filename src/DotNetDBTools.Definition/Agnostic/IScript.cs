using System;
using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Agnostic;

public interface IScript : IBaseScript
{
    public Func<DbmsKind, string> Text { get; }
}
