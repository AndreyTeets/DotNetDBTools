﻿using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.PostgreSQL;

public class PostgreSQLProcedure : DbObject
{
    public CodePiece CreateStatement { get; set; }
}
