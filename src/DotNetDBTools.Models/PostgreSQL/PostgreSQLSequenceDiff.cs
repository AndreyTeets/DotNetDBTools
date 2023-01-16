using System;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.PostgreSQL;

public class PostgreSQLSequenceDiff
{
    public Guid SequenceID { get; set; }
    public string NewSequenceName { get; set; }
    public string OldSequenceName { get; set; }

    public DataType DataTypeToSet { get; set; }
    public PostgreSQLSequenceOptions OptionsToSet { get; set; }
    public (string TableName, string ColumnName) OwnedByToSet { get; set; }
    public (string TableName, string ColumnName) OwnedByToDrop { get; set; }
}
