using System.Collections.Generic;
using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.PostgreSQL;

public class Index : BaseIndex
{
    public Index(string id) : base(id) { }

    public IEnumerable<string> IncludeColumns { get; set; }
    public IndexMethod Method { get; set; }

    /// <summary>
    /// If specified will be used instead of Columns property.
    /// Columns property must not be specified or exception will be thrown.
    /// Will appear in sql as (Expression).
    /// </summary>
    public string Expression { get; set; }
}
