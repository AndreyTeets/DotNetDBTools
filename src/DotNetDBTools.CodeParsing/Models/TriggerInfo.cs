namespace DotNetDBTools.CodeParsing.Models;

public class TriggerInfo : ObjectInfo
{
    public string Table { get; set; }
    public string CreateStatement { get; set; }
}
