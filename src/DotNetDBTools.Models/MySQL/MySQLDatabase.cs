﻿using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MySQL;

public class MySQLDatabase : Database
{
    public MySQLDatabase()
    {
        Kind = DatabaseKind.MySQL;
    }

    public List<MySQLFunction> Functions { get; set; } = new();
    public List<MySQLProcedure> Procedures { get; set; } = new();
}
