using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MSSQL;

public interface IFunction : IDbObject
{
    public string CreateStatement { get; }
}
