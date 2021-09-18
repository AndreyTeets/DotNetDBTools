namespace DotNetDBTools.Definition.MSSQL
{
    public interface IUserDefinedType : IDbObject, IDbType
    {
        public IDbType UnderlyingType { get; }
        public bool Nullable { get; }
    }
}
