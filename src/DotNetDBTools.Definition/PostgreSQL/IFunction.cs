using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.PostgreSQL;

public interface IFunction : IDbObject
{
    public string CreateStatement { get; }
}
