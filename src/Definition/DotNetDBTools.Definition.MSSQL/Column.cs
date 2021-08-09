namespace DotNetDBTools.Definition.MSSQL
{
    public class Column : BaseColumn
    {
        public Column(string id) : base(id) { }

        public string ComputeCode { get; set; }
    }
}
