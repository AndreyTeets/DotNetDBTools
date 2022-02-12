﻿using System;
using System.Collections.Generic;

namespace DotNetDBTools.Definition.Core;

public abstract class BaseIndex : IDbObject
{
    private readonly Guid _id;
    protected BaseIndex(string id)
    {
        _id = new Guid(id);
    }

    public Guid ID => _id;
    public IEnumerable<string> Columns { get; set; }
    public IEnumerable<string> IncludeColumns { get; set; }
    public bool Unique { get; set; }
}
