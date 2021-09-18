namespace DotNetDBTools.Definition.MSSQL
{
    public interface IProcedure : IDbObject
    {
        public string Code { get; }
    }
}
