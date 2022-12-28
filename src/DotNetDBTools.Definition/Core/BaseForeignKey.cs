using System;
using System.Collections.Generic;

namespace DotNetDBTools.Definition.Core;

public abstract class BaseForeignKey : IDbObject
{
    private readonly Guid _id;
    protected BaseForeignKey(string id)
    {
        _id = new Guid(id);
    }

    public Guid DNDBT_OBJECT_ID => _id;
    public IEnumerable<string> ThisColumns { get; set; }
    public string ReferencedTable { get; set; }
    public IEnumerable<string> ReferencedTableColumns { get; set; }
}
