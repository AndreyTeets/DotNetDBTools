﻿using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.PostgreSQL;

public class ForeignKey : BaseForeignKey
{
    public ForeignKey(string id) : base(id) { }

    public ForeignKeyActions OnUpdate { get; set; }
    public ForeignKeyActions OnDelete { get; set; }
}
