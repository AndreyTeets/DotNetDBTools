namespace DotNetDBTools.Models.Core;

public class Column : DbObject
{
    public DataType DataType { get; set; }
    public bool Nullable { get; set; }
    public bool Identity { get; set; }
    public object Default { get; set; }
}
