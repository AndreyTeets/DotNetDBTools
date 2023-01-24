using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.SQLite;

public class SQLiteTableDiff : TableDiff
{
    public Table NewTableToDefine { get; set; }
    public List<string> CommonColumnsNewNames { get; set; }
    public List<string> CommonColumnsOldNames { get; set; }
}
