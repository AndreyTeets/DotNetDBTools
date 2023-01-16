using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.PostgreSQL;

public class PostgreSQLColumn : Column
{
    public string IdentityGenerationKind { get; set; }
    public PostgreSQLSequenceOptions IdentitySequenceOptions { get; set; }
}
