using System;
using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Agnostic;

public interface IView : IBaseView
{
    /// <summary>
    /// Full create view statement.
    /// </summary>
    public Func<DbmsKind, string> Code { get; }
}
