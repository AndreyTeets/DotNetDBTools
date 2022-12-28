using System;
using System.Collections.Generic;

namespace DotNetDBTools.Definition.Core;

public abstract class BasePrimaryKey : IDbObject
{
    private readonly Guid _id;
    protected BasePrimaryKey(string id)
    {
        _id = new Guid(id);
    }

    public Guid DNDBT_OBJECT_ID => _id;
    public IEnumerable<string> Columns { get; set; }
}
