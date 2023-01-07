using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MSSQL;

public interface IProcedure : IDbObject
{
    /// <summary>
    /// Full create procedure statement.
    /// </summary>
    public string Code { get; }
}
