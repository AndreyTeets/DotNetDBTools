namespace DotNetDBTools.Models.Core;

public class Script : DbObject
{
    public ScriptKind Kind { get; set; }
    public long MinDbVersionToExecute { get; set; }
    public long MaxDbVersionToExecute { get; set; }
    public CodePiece CodePiece { get; set; }
}
