using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Common;

public interface ISpecificDbmsView : IBaseView
{
    public string CreateStatement { get; }
}
