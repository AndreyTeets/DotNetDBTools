using System.Collections.Generic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Models.PostgreSQL;

public class PostgreSQLDatabaseDiff : DatabaseDiff
{
    public IEnumerable<PostgreSQLCompositeType> AddedCompositeTypes { get; set; }
    public IEnumerable<PostgreSQLCompositeType> RemovedCompositeTypes { get; set; }
    public IEnumerable<PostgreSQLCompositeTypeDiff> ChangedCompositeTypes { get; set; }

    public IEnumerable<PostgreSQLDomainType> AddedDomainTypes { get; set; }
    public IEnumerable<PostgreSQLDomainType> RemovedDomainTypes { get; set; }
    public IEnumerable<PostgreSQLDomainTypeDiff> ChangedDomainTypes { get; set; }

    public IEnumerable<PostgreSQLEnumType> AddedEnumTypes { get; set; }
    public IEnumerable<PostgreSQLEnumType> RemovedEnumTypes { get; set; }
    public IEnumerable<PostgreSQLEnumTypeDiff> ChangedEnumTypes { get; set; }

    public IEnumerable<PostgreSQLRangeType> AddedRangeTypes { get; set; }
    public IEnumerable<PostgreSQLRangeType> RemovedRangeTypes { get; set; }
    public IEnumerable<PostgreSQLRangeTypeDiff> ChangedRangeTypes { get; set; }

    public IEnumerable<PostgreSQLFunction> FunctionsToCreate { get; set; }
    public IEnumerable<PostgreSQLFunction> FunctionsToDrop { get; set; }

    public IEnumerable<PostgreSQLProcedure> ProceduresToCreate { get; set; }
    public IEnumerable<PostgreSQLProcedure> ProceduresToDrop { get; set; }
}
