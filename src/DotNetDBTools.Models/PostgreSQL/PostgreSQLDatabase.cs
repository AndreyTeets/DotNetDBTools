using System.Collections.Generic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Models.PostgreSQL;

public class PostgreSQLDatabase : Database
{
    public PostgreSQLDatabase()
    {
        Kind = DatabaseKind.PostgreSQL;
    }

    public List<PostgreSQLCompositeType> CompositeTypes { get; set; }
    public List<PostgreSQLDomainType> DomainTypes { get; set; }
    public List<PostgreSQLEnumType> EnumTypes { get; set; }
    public List<PostgreSQLRangeType> RangeTypes { get; set; }

    public List<PostgreSQLFunction> Functions { get; set; }
    public List<PostgreSQLProcedure> Procedures { get; set; }

    public override void InitializeAdditionalProperties()
    {
        CompositeTypes = new();
        DomainTypes = new();
        EnumTypes = new();
        RangeTypes = new();
        Functions = new();
        Procedures = new();
    }
}
