using System.Collections.Generic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Models.PostgreSQL;

public class PostgreSQLDatabaseDiff : DatabaseDiff
{
    public List<PostgreSQLCompositeType> AddedCompositeTypes { get; set; }
    public List<PostgreSQLCompositeType> RemovedCompositeTypes { get; set; }
    public List<PostgreSQLCompositeTypeDiff> ChangedCompositeTypes { get; set; }

    public List<PostgreSQLDomainType> AddedDomainTypes { get; set; }
    public List<PostgreSQLDomainType> RemovedDomainTypes { get; set; }
    public List<PostgreSQLDomainTypeDiff> ChangedDomainTypes { get; set; }

    public List<PostgreSQLEnumType> AddedEnumTypes { get; set; }
    public List<PostgreSQLEnumType> RemovedEnumTypes { get; set; }
    public List<PostgreSQLEnumTypeDiff> ChangedEnumTypes { get; set; }

    public List<PostgreSQLRangeType> AddedRangeTypes { get; set; }
    public List<PostgreSQLRangeType> RemovedRangeTypes { get; set; }
    public List<PostgreSQLRangeTypeDiff> ChangedRangeTypes { get; set; }

    public List<PostgreSQLFunction> FunctionsToCreate { get; set; }
    public List<PostgreSQLFunction> FunctionsToDrop { get; set; }

    public List<PostgreSQLProcedure> ProceduresToCreate { get; set; }
    public List<PostgreSQLProcedure> ProceduresToDrop { get; set; }
}
