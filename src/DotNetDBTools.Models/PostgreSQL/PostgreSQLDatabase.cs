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

    public List<PostgreSQLSequence> Sequences { get; set; } = new();

    public List<PostgreSQLCompositeType> CompositeTypes { get; set; } = new();
    public List<PostgreSQLDomainType> DomainTypes { get; set; } = new();
    public List<PostgreSQLEnumType> EnumTypes { get; set; } = new();
    public List<PostgreSQLRangeType> RangeTypes { get; set; } = new();

    public List<PostgreSQLFunction> Functions { get; set; } = new();
    public List<PostgreSQLProcedure> Procedures { get; set; } = new();
}
