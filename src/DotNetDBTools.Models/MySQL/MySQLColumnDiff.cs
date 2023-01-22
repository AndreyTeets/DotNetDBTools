using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MySQL;

public class MySQLColumnDiff : ColumnDiff
{
    public Column DefinitionToSet { get; set; }
}
