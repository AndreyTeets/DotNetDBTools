using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.PostgreSQL;

public interface IProcedure : IDbObject
{
    /// <summary>
    /// Full create procedure statement.
    /// </summary>
    public string Code { get; }
}
