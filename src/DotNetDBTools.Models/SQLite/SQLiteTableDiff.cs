using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.SQLite;

public class SQLiteTableDiff : TableDiff
{
    public Table NewTable { get; set; }
    public Table OldTable { get; set; }
}
