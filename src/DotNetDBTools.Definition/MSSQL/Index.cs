using System.Collections.Generic;
using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MSSQL;

public class Index : BaseIndex
{
    public Index(string id) : base(id) { }

    public IEnumerable<string> IncludeColumns { get; set; }
}
