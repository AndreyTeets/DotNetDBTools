using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MySQL;

public interface IFunction : IDbObject
{
    public string CreateStatement { get; }
}
