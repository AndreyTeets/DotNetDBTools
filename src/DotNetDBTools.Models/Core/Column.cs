namespace DotNetDBTools.Models.Core;

public class Column : DbObject
{
    public DataType DataType { get; set; }
    public bool NotNull { get; set; }
    public bool Identity { get; set; }
    public object Default { get; set; }
}
