namespace DotNetDBTools.CodeParsing.Models;

public class SequenceInfo : ObjectInfo
{
    public string DataType { get; set; }
    public long? StartWith { get; set; }
    public long? IncrementBy { get; set; }
    public long? MinValue { get; set; }
    public long? MaxValue { get; set; }
    public long? Cache { get; set; }
    public bool? Cycle { get; set; }
    public string OwnedByTableName { get; set; }
    public string OwnedByColumnName { get; set; }
}
