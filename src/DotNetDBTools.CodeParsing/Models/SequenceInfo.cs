namespace DotNetDBTools.CodeParsing.Models;

public class SequenceInfo : ObjectInfo
{
    public string DataType { get; set; }
    public SequenceOptions Options { get; set; }
    public string OwnedByTableName { get; set; }
    public string OwnedByColumnName { get; set; }
}
