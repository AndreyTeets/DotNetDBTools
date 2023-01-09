using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.PostgreSQL;

public class Column : BaseColumn
{
    public Column(string id) : base(id) { }

    public IdentityGenerationKind IdentityGenerationKind { get; set; }
    public SequenceOptions IdentitySequenceOptions { get; set; } = new();
}
