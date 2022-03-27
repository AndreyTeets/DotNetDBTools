using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MSSQL.UserDefinedTypes;

public interface IUserDefinedType : IDbObject, IDataType
{
    public IDataType UnderlyingType { get; }
    public bool NotNull { get; }
}
