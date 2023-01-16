using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.PostgreSQL;

public class PostgreSQLSequence : DbObject
{
    public DataType DataType { get; set; }
    public PostgreSQLSequenceOptions Options { get; set; }
    public (string TableName, string ColumnName) OwnedBy { get; set; }
}
