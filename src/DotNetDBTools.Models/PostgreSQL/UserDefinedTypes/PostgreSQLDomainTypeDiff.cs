using System;
using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

public class PostgreSQLDomainTypeDiff
{
    public Guid TypeID { get; set; }
    public string NewTypeName { get; set; }
    public string OldTypeName { get; set; }

    public bool? NotNullToSet { get; set; }
    public CodePiece DefaultToSet { get; set; }
    public CodePiece DefaultToDrop { get; set; }

    public List<CheckConstraint> CheckConstraintsToCreate { get; set; } = new();
    public List<CheckConstraint> CheckConstraintsToDrop { get; set; } = new();
}
