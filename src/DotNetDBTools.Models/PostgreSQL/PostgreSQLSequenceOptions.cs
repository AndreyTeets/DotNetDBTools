namespace DotNetDBTools.Models.PostgreSQL;

public class PostgreSQLSequenceOptions
{
    public long? StartWith { get; set; }
    public long? IncrementBy { get; set; }
    public long? MinValue { get; set; }
    public long? MaxValue { get; set; }
    public long? Cache { get; set; }
    public bool? Cycle { get; set; }
}
