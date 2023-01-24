using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Analysis.Extensions.MSSQL;

public static class MSSQLDbObjectsExtensions
{
    /// <summary>
    /// Creates empty table diff model and sets TableID and [New|Old]TableName
    /// </summary>
    public static MSSQLTableDiff CreateEmptyTableDiff(this Table table)
    {
        return new()
        {
            ID = table.ID,
            NewName = table.Name,
            OldName = table.Name,
        };
    }

    /// <summary>
    /// Creates empty column diff model and sets ColumnID and [New|Old]ColumnName
    /// </summary>
    public static MSSQLColumnDiff CreateEmptyColumnDiff(this Column column)
    {
        return new()
        {
            ID = column.ID,
            NewName = column.Name,
            OldName = column.Name,
        };
    }
}
