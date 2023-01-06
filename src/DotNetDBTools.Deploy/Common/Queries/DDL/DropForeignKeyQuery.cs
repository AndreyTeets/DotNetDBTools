using System.Collections.Generic;
using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Generation;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Common.Queries.DDL;

internal abstract class DropForeignKeyQuery : NoParametersQuery
{
    public override string Sql => _sql;
    private readonly string _sql;

    public DropForeignKeyQuery(ForeignKey fk)
    {
        _sql = GetSql(fk);
    }

    protected abstract string GetSql(ForeignKey fk);

    protected string GetSqlBase<TTable, TTableDiff>(ForeignKey fk)
        where TTable : Table, new()
        where TTableDiff : TableDiff, new()
    {
        TTable table = new() { Name = fk.ThisTableName };
        TTableDiff tableDiff = new()
        {
            NewTable = table,
            OldTable = table,
            ForeignKeysToDrop = new List<ForeignKey>() { fk },
        };
        return GenerationManager.GenerateSqlAlterStatement(tableDiff);
    }
}
