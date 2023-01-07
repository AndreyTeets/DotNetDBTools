using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Common;

public interface ISpecificDbmsScript : IBaseScript
{
    public string Text { get; }
}
