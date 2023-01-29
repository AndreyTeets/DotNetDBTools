using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Analysis.Extensions.MySQL;

public static class MySQLDbObjectsExtensions
{
    /// <summary>
    /// Creates empty table diff model and sets TableID and [New|Old]TableName
    /// </summary>
    public static MySQLTableDiff CreateEmptyTableDiff(this Table table)
    {
        return new()
        {
            ID = table.ID,
            NewName = table.Name,
            OldName = table.Name,
        };
    }
}
