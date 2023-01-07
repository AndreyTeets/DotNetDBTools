namespace DotNetDBTools.Models.Core;

public abstract class View : DbObject
{
    public CodePiece CreateStatement { get; set; }
}
