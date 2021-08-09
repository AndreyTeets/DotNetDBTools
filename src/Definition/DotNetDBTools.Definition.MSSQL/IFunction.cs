namespace DotNetDBTools.Definition.MSSQL
{
    public interface IFunction : IDbObject
    {
        public string Code { get; }
    }
}
