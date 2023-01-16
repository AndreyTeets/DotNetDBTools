namespace DotNetDBTools.Definition.PostgreSQL;

public class SequenceOptions
{
    public long StartWith { get; set; } = 1;
    public long IncrementBy { get; set; } = 1;
    public long MinValue { get; set; } = 1;
    public long MaxValue { get; set; } = int.MaxValue;
    public long Cache { get; set; } = 1;
    public bool Cycle { get; set; } = false;
}
