﻿using System;

namespace DotNetDBTools.Definition.Core;

public abstract class BaseTrigger : IDbObject
{
    private readonly Guid _id;
    protected BaseTrigger(string id)
    {
        _id = new Guid(id);
    }

    public Guid DNDBT_OBJECT_ID => _id;
}
