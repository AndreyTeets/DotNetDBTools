using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MSSQL;

public class Column : BaseColumn
{
    public Column(string id) : base(id) { }

    public string DefaultConstraintName { get; set; }
    public string ComputeCode { get; set; }
}
