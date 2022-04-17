namespace DotNetDBTools.Models.Core;

public class ColumnDiff
{
    public Column NewColumn { get; set; }
    public Column OldColumn { get; set; }
    public bool DataTypeChanged { get; set; } = false;
}
