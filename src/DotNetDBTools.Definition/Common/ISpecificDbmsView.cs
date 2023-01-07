using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Common;

public interface ISpecificDbmsView : IBaseView
{
    /// <summary>
    /// Full create view statement.
    /// </summary>
    public string Code { get; }
}
