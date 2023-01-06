using DotNetDBTools.Generation;
using DotNetDBTools.Models;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Queries.DDL;

internal class AlterTableQuery : NoParametersQuery
{
    public override string Sql => _sql;
    private readonly string _sql;

    public AlterTableQuery(TableDiff tableDiff)
    {
        _sql = GetSql(tableDiff);
    }

    protected virtual string GetSql(TableDiff tableDiff)
    {
        TableDiff tableDiffWithoutForeignKeys = tableDiff.CopyModel();
        tableDiffWithoutForeignKeys.ForeignKeysToCreate.Clear();
        tableDiffWithoutForeignKeys.ForeignKeysToDrop.Clear();
        return GenerationManager.GenerateSqlAlterStatement(tableDiffWithoutForeignKeys);
    }
}
