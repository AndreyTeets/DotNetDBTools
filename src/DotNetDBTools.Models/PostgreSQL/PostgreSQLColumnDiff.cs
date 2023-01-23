using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.PostgreSQL;

public class PostgreSQLColumnDiff : ColumnDiff
{
    public string IdentityGenerationKindToSet { get; set; }
    public PostgreSQLSequenceOptions IdentitySequenceOptionsToSet { get; set; }
}
