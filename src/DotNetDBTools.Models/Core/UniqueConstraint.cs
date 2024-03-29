﻿using System.Collections.Generic;

namespace DotNetDBTools.Models.Core;

public class UniqueConstraint : DbObject
{
    public List<string> Columns { get; set; } = new();

    public List<Column> DependsOn { get; set; } = new();
}
