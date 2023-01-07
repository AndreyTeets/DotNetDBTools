using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MySQL;

public interface IFunction : IDbObject
{
    /// <summary>
    /// Full create function statement.
    /// </summary>
    public string Code { get; }
}
