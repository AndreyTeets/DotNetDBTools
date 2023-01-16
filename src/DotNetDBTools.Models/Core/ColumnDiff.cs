using System;

namespace DotNetDBTools.Models.Core;

public class ColumnDiff
{
    public Guid ColumnID { get; set; }
    public string NewColumnName { get; set; }
    public string OldColumnName { get; set; }

    public Column NewColumn { get; set; }
    public Column OldColumn { get; set; }

    public DataType DataTypeToSet { get; set; }
    public bool? NotNullToSet { get; set; }
    public CodePiece DefaultToSet { get; set; }
    public CodePiece DefaultToDrop { get; set; }
}
