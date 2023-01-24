using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.PostgreSQL;

public class PostgreSQLSequenceDiff : DbObjectDiff
{
    public DataType DataTypeToSet { get; set; }
    public PostgreSQLSequenceOptions OptionsToSet { get; set; }
    public (string TableName, string ColumnName) OwnedByToSet { get; set; }
    public (string TableName, string ColumnName) OwnedByToDrop { get; set; }
}
