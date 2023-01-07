using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MySQL;

public interface IProcedure : IDbObject
{
    public string CreateStatement { get; }
}
