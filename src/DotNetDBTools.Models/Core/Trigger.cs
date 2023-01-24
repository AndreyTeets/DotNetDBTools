namespace DotNetDBTools.Models.Core;

public abstract class Trigger : DbObject
{
    public CodePiece CreateStatement { get; set; }
}
