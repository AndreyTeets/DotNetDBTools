﻿using System;

namespace DotNetDBTools.Definition.Core;

public abstract class BaseCheckConstraint : IDbObject
{
    private readonly Guid _id;
    protected BaseCheckConstraint(string id)
    {
        _id = new Guid(id);
    }

    public Guid DNDBT_OBJECT_ID => _id;
}
