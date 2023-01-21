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
            TableID = table.ID,
            NewTableName = table.Name,
            OldTableName = table.Name,
            NewTable = new MSSQLTable() { Name = table.Name },
            OldTable = new MSSQLTable() { Name = table.Name },
        };
    }

    /// <summary>
    /// Creates empty column diff model and sets ColumnID and [New|Old]ColumnName
    /// </summary>
    public static MSSQLColumnDiff CreateEmptyColumnDiff(this Column column)
    {
        return new()
        {
            ColumnID = column.ID,
            NewColumnName = column.Name,
            OldColumnName = column.Name,
        };
    }
}
