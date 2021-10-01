using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MSSQL
{
    public interface IUserDefinedType : IDbObject, IDataType
    {
        public IDataType UnderlyingType { get; }
        public bool Nullable { get; }
    }
}
