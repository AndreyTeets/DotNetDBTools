namespace DotNetDBTools.Models.Core;

public class ColumnDiff : DbObjectDiff
{
    public DataType DataTypeToSet { get; set; }
    public bool? NotNullToSet { get; set; }
    public bool? IdentityToSet { get; set; }
    public CodePiece DefaultToSet { get; set; }
    public CodePiece DefaultToDrop { get; set; }
}
