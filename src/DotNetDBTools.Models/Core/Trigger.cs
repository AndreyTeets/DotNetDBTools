namespace DotNetDBTools.Models.Core;

public abstract class Trigger : DbObject
{
    public string TableName { get; set; }
    public CodePiece CreateStatement { get; set; }
}
