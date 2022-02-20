namespace DotNetDBTools.CodeParsing.Core.Models;

public class ScriptInfo : ObjectInfo
{
    public ScriptType Type { get; set; }
    public long MinDbVersionToExecute { get; set; }
    public long MaxDbVersionToExecute { get; set; }
    public string Code { get; set; }
}
