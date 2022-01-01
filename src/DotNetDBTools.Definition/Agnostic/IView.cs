using System;
using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Agnostic
{
    public interface IView : IBaseView
    {
        public Func<DbmsKind, string> Code { get; }
    }
}
