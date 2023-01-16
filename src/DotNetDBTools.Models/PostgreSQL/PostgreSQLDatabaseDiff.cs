using System.Collections.Generic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Models.PostgreSQL;

public class PostgreSQLDatabaseDiff : DatabaseDiff
{
    public List<PostgreSQLSequence> SequencesToCreate { get; set; } = new();
    public List<PostgreSQLSequence> SequencesToDrop { get; set; } = new();
    public List<PostgreSQLSequenceDiff> SequencesToAlter { get; set; } = new();

    public List<PostgreSQLCompositeType> CompositeTypesToCreate { get; set; } = new();
    public List<PostgreSQLCompositeType> CompositeTypesToDrop { get; set; } = new();

    public List<PostgreSQLDomainType> DomainTypesToCreate { get; set; } = new();
    public List<PostgreSQLDomainType> DomainTypesToDrop { get; set; } = new();
    public List<PostgreSQLDomainTypeDiff> DomainTypesToAlter { get; set; } = new();

    public List<PostgreSQLEnumType> EnumTypesToCreate { get; set; } = new();
    public List<PostgreSQLEnumType> EnumTypesToDrop { get; set; } = new();

    public List<PostgreSQLRangeType> RangeTypesToCreate { get; set; } = new();
    public List<PostgreSQLRangeType> RangeTypesToDrop { get; set; } = new();

    public List<PostgreSQLFunction> FunctionsToCreate { get; set; } = new();
    public List<PostgreSQLFunction> FunctionsToDrop { get; set; } = new();

    public List<PostgreSQLProcedure> ProceduresToCreate { get; set; } = new();
    public List<PostgreSQLProcedure> ProceduresToDrop { get; set; } = new();
}
