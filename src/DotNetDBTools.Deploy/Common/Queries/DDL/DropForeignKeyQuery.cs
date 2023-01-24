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

    protected string GetSqlBase<TTableDiff>(ForeignKey fk)
        where TTableDiff : TableDiff, new()
    {
        TTableDiff tableDiff = new()
        {
            TableID = fk.Parent.ID,
            NewTableName = fk.Parent.Name,
            OldTableName = fk.Parent.Name,
            ForeignKeysToDrop = new List<ForeignKey>() { fk },
        };
        return GenerationManager.GenerateSqlAlterStatement(tableDiff);
    }
}
