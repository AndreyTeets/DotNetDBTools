using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Analysis.Extensions.PostgreSQL;

public static class PostgreSQLDbObjectsExtensions
{
    /// <summary>
    /// Rules to get PostgreSQL object's direct dependencies.
    /// </summary>
    public static IEnumerable<DbObject> GetDependencies(this DbObject dbObject)
    {
        return dbObject switch
        {
            PostgreSQLSequence x => new List<DbObject>(),
            PostgreSQLCompositeType x => x.Attributes
                .Select(y => (IEnumerable<DbObject>)y.DataType.DependsOn)
                .Aggregate((y1, y2) => y1.Concat(y2)),
            PostgreSQLDomainType x => x.UnderlyingType.DependsOn,
            PostgreSQLEnumType x => new List<DbObject>(),
            PostgreSQLRangeType x => x.Subtype.DependsOn,
            PostgreSQLTable x => x.Columns
                .Select(y => (IEnumerable<DbObject>)y.DataType.DependsOn)
                .Aggregate((y1, y2) => y1.Concat(y2)),
            PostgreSQLColumn x => x.DataType.DependsOn,
            PostgreSQLView x => x.CreateStatement.DependsOn,
            PostgreSQLFunction x => x.CreateStatement.DependsOn,
            PostgreSQLProcedure x => x.CreateStatement.DependsOn,
            _ => throw new InvalidOperationException($"Invalid dbObject type for getting dependencies: {dbObject.GetType()}"),
        };
    }

    /// <summary>
    /// Creates empty table diff model and sets TableID and [New|Old]TableName
    /// </summary>
    public static PostgreSQLTableDiff CreateEmptyTableDiff(this Table table)
    {
        return new()
        {
            ID = table.ID,
            NewName = table.Name,
            OldName = table.Name,
        };
    }

    /// <summary>
    /// Creates empty column diff model and sets ColumnID and [New|Old]ColumnName
    /// </summary>
    public static PostgreSQLColumnDiff CreateEmptyColumnDiff(this Column column)
    {
        return new()
        {
            ID = column.ID,
            NewName = column.Name,
            OldName = column.Name,
        };
    }

    /// <summary>
    /// Creates empty sequence diff model and sets SequenceID and [New|Old]SequenceName
    /// </summary>
    public static PostgreSQLSequenceDiff CreateEmptySequenceDiff(this PostgreSQLSequence sequence)
    {
        return new()
        {
            ID = sequence.ID,
            NewName = sequence.Name,
            OldName = sequence.Name,
        };
    }

    /// <summary>
    /// Creates empty domain type diff model and sets TypeID and [New|Old]TypeName
    /// </summary>
    public static PostgreSQLDomainTypeDiff CreateEmptyDomainTypeDiff(this PostgreSQLDomainType type)
    {
        return new()
        {
            ID = type.ID,
            NewName = type.Name,
            OldName = type.Name,
        };
    }
}
