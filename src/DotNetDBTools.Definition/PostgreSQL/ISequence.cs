using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.PostgreSQL;

public interface ISequence : IDbObject
{
    public IDataType DataType { get; }
    public SequenceOptions Options { get; }
    public (string TableName, string ColumnName) OwnedBy { get; }
}
