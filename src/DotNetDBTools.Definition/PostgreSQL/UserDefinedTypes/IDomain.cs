using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.PostgreSQL.UserDefinedTypes;

public interface IDomain : IDbObject, IDataType
{
    public IDataType UnderlyingType { get; }
    public bool NotNull { get; }
    public object Default { get; }
    public bool DefaultIsFunction { get; }
}
