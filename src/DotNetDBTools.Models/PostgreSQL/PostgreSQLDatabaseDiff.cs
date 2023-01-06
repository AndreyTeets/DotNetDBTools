using System.Collections.Generic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Models.PostgreSQL;

public class PostgreSQLDatabaseDiff : DatabaseDiff
{
    public List<PostgreSQLCompositeType> AddedCompositeTypes { get; set; } = new();
    public List<PostgreSQLCompositeType> RemovedCompositeTypes { get; set; } = new();
    public List<PostgreSQLCompositeTypeDiff> ChangedCompositeTypes { get; set; } = new();

    public List<PostgreSQLDomainType> AddedDomainTypes { get; set; } = new();
    public List<PostgreSQLDomainType> RemovedDomainTypes { get; set; } = new();
    public List<PostgreSQLDomainTypeDiff> ChangedDomainTypes { get; set; } = new();

    public List<PostgreSQLEnumType> AddedEnumTypes { get; set; } = new();
    public List<PostgreSQLEnumType> RemovedEnumTypes { get; set; } = new();
    public List<PostgreSQLEnumTypeDiff> ChangedEnumTypes { get; set; } = new();

    public List<PostgreSQLRangeType> AddedRangeTypes { get; set; } = new();
    public List<PostgreSQLRangeType> RemovedRangeTypes { get; set; } = new();
    public List<PostgreSQLRangeTypeDiff> ChangedRangeTypes { get; set; } = new();

    public List<PostgreSQLFunction> FunctionsToCreate { get; set; } = new();
    public List<PostgreSQLFunction> FunctionsToDrop { get; set; } = new();

    public List<PostgreSQLProcedure> ProceduresToCreate { get; set; } = new();
    public List<PostgreSQLProcedure> ProceduresToDrop { get; set; } = new();
}
