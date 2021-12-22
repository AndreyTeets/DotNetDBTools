namespace DotNetDBTools.Definition.PostgreSQL
{
    public interface ITypedTable : ITable
    {
        public string OfType { get; }
    }
}
