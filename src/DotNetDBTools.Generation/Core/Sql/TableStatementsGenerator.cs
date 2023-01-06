using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Generation.Core.Sql;

internal abstract class TableStatementsGenerator<TDbObject, TTableDiff>
    : StatementsGenerator<TDbObject>, IAlterStatementGenerator
    where TDbObject : Table
    where TTableDiff : TableDiff
{
    public string GetAlterSql(TableDiff tableDiff) => GetAlterSqlImpl((TTableDiff)tableDiff);

    protected abstract string GetAlterSqlImpl(TTableDiff tableDiff);
}
